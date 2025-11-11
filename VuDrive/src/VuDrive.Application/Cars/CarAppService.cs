using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace VuDrive.Cars;

public interface ICarAppService :
    ICrudAppService<CarDto, Guid, CarsListInput, CreateUpdateCarDto, CreateUpdateCarDto>
{
    Task<ImportCarsResultDto> ImportCsvAsync(ImportCarsUploadDto input);
}

public class CarAppService
  : CrudAppService<Car, CarDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateCarDto, CreateUpdateCarDto>,
    ICarAppService
{
    public CarAppService(IRepository<Car, Guid> repo) : base(repo) { }

    public override async Task<CarDto> CreateAsync(CreateUpdateCarDto input)
    {
        Normalize(input);
        ValidateRequired(input);
        return await base.CreateAsync(input);
    }

    public override async Task<CarDto> UpdateAsync(Guid id, CreateUpdateCarDto input)
    {
        Normalize(input);
        ValidateRequired(input);
        return await base.UpdateAsync(id, input);
    }

    private static void Normalize(CreateUpdateCarDto input)
    {
        input.Mark = input.Mark?.Trim() ?? string.Empty;
        input.Model = input.Model?.Trim() ?? string.Empty;
        input.SpecificationModel = string.IsNullOrWhiteSpace(input.SpecificationModel)
            ? null
            : input.SpecificationModel.Trim();
    }

    private static void ValidateRequired(CreateUpdateCarDto input)
    {
        if (string.IsNullOrWhiteSpace(input.Mark))
            throw new UserFriendlyException("Mark is required.");
        if (string.IsNullOrWhiteSpace(input.Model))
            throw new UserFriendlyException("Model is required.");
    }


    //------------------------------------------------------------------------------------------------------------------------
    public async Task<ImportCarsResultDto> ImportCsvAsync(Stream csvStream, CancellationToken ct = default)
    {
        var result = new ImportCarsResultDto();

        // CSV setup
        var cfg = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            DetectDelimiter = true,           // will still handle comma; safe to leave on
            IgnoreBlankLines = true,
            BadDataFound = null,
            MissingFieldFound = null,
            TrimOptions = TrimOptions.Trim
        };

        // Read all rows first (small files are fine; adjust if you expect very large files)
        List<CsvCarRow> rows;
        using (var reader = new StreamReader(csvStream, leaveOpen: true))
        using (var csv = new CsvReader(reader, cfg))
        {
            rows = csv.GetRecords<CsvCarRow>().ToList();
        }
        csvStream.Position = 0;

        // Normalize rows and build keys
        static string Norm(string? s) => (s ?? string.Empty).Trim();
        static string NormSpec(string? s) => string.IsNullOrWhiteSpace(s) ? string.Empty : s.Trim();

        var normalizedRows = rows.Select(r => new
        {
            Mark = Norm(r.brand),
            Model = Norm(r.series),
            Spec = NormSpec(r.generation),
            Years = (r.manufacturing_years ?? string.Empty)
                        .Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .Select(y => y.Trim())
                        .Where(y => !string.IsNullOrWhiteSpace(y))
                        .ToList()
        })
        .Where(x => !string.IsNullOrWhiteSpace(x.Mark) && !string.IsNullOrWhiteSpace(x.Model))
        .ToList();

        // Deduplicate inside the CSV itself
        var csvKeyComparer = StringComparer.OrdinalIgnoreCase;
        var seenInCsv = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        string Key(string mark, string model, string spec) => $"{mark}||{model}||{spec}";

        normalizedRows = normalizedRows
            .Where(x => seenInCsv.Add(Key(x.Mark, x.Model, x.Spec)))
            .ToList();

        // Preload existing car keys from DB to skip duplicates efficiently
        // If your dataset is large, you may prefer a batched lookup instead.
        var existing = await Repository.GetQueryableAsync();
        var existingKeys = existing
            .Select(c => new
            {
                c.Mark,
                c.Model,
                Spec = c.SpecificationModel
            })
            .AsEnumerable() // switch to client to use our normalization reliably across providers
            .Select(c => Key(Norm(c.Mark), Norm(c.Model), NormSpec(c.Spec)))
            .ToHashSet(csvKeyComparer);

        foreach (var x in normalizedRows)
        {
            ct.ThrowIfCancellationRequested();

            var key = Key(x.Mark, x.Model, x.Spec);
            if (existingKeys.Contains(key))
            {
                result.SkippedDuplicates++;
                continue;
            }

            try
            {
                var dto = new CreateUpdateCarDto
                {
                    Mark = x.Mark,
                    Model = x.Model,
                    SpecificationModel = string.IsNullOrWhiteSpace(x.Spec) ? null : x.Spec,
                    YearsBuilt = x.Years // EF conversion will store comma-separated in DB
                };

                await CreateAsync(dto); // uses your CrudAppService create path + validations

                existingKeys.Add(key);
                result.Inserted++;
            }
            catch
            {
                // If a single row fails validation/unexpected issue, skip and continue
                result.SkippedInvalid++;
            }
        }

        return result;
    }

    public async Task<ImportCarsResultDto> ImportCsvAsync(ImportCarsUploadDto input)
    {
        using var ms = new MemoryStream(input.Content ?? Array.Empty<byte>());
        return await ImportCsvAsync(ms); // your existing Stream-based method
    }

    public async Task<PagedResultDto<CarDto>> GetListAsync(CarsListInput input)
    {
        var q = await Repository.GetQueryableAsync();

        string? m = string.IsNullOrWhiteSpace(input.Mark) ? null : input.Mark!.ToLower();
        string? mdl = string.IsNullOrWhiteSpace(input.Model) ? null : input.Model!.ToLower();
        string? sp = string.IsNullOrWhiteSpace(input.Spec) ? null : input.Spec!.ToLower();

        q = q
            .WhereIf(m != null, x => EF.Functions.Like(x.Mark.ToLower(), $"%{m}%"))
            .WhereIf(mdl != null, x => EF.Functions.Like(x.Model.ToLower(), $"%{mdl}%"))
            .WhereIf(sp != null, x => EF.Functions.Like((x.SpecificationModel ?? string.Empty).ToLower(), $"%{sp}%"));

        var total = await AsyncExecuter.CountAsync(q);

        var sorting = string.IsNullOrWhiteSpace(input.Sorting) ? "Mark, Model" : input.Sorting;
        var page = q.OrderBy(sorting) // needs System.Linq.Dynamic.Core
                    .PageBy(input.SkipCount, input.MaxResultCount);

        var list = await AsyncExecuter.ToListAsync(page);
        var dtos = ObjectMapper.Map<List<Car>, List<CarDto>>(list);

        return new PagedResultDto<CarDto>(total, dtos);
    }
}



file sealed class CsvCarRow
{
    public string? brand { get; set; }
    public string? series { get; set; }
    public string? generation { get; set; }
    public string? manufacturing_years { get; set; }
}

public sealed class ImportCarsResultDto
{
    public int Inserted { get; set; }
    public int SkippedDuplicates { get; set; }
    public int SkippedInvalid { get; set; }
}
