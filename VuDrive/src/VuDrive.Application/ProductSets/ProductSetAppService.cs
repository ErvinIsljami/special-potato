using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace VuDrive.ProductSets;

public interface IProductSetAppService :
    ICrudAppService<ProductSetDto, Guid, ProductSetsListInput, CreateUpdateProductSetDto, CreateUpdateProductSetDto>
{ }


[Authorize]
public class ProductSetAppService :
    CrudAppService<ProductSet, ProductSetDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateProductSetDto>,
    IProductSetAppService
{
    private readonly IRepository<ProductSetCar, Guid> _linkRepo;

    public ProductSetAppService(
        IRepository<ProductSet, Guid> repo,
        IRepository<ProductSetCar, Guid> linkRepo
    ) : base(repo)
    {
        _linkRepo = linkRepo;
    }

    public override async Task<ProductSetDto> GetAsync(Guid id)
    {
        var dto = await base.GetAsync(id);

        var q = await _linkRepo.GetQueryableAsync();
        dto.CompatibleCarIds =  q
            .Where(x => x.ProductSetId == id)
            .Select(x => x.CarId)
            .ToList();

        return dto;
    }

    public override async Task<ProductSetDto> CreateAsync(CreateUpdateProductSetDto input)
    {
        Normalize(input);
        Validate(input);

        // Create the ProductSet (saves entity)
        var created = await base.CreateAsync(input);

        // Insert links
        if (input.CompatibleCarIds != null && input.CompatibleCarIds.Count > 0)
        {
            foreach (var carId in input.CompatibleCarIds.Distinct())
            {
                await _linkRepo.InsertAsync(
                    new ProductSetCar(Guid.NewGuid(), created.Id, carId),
                    autoSave: true
                );
            }
        }

        // Return with CompatibleCarIds populated
        return await GetAsync(created.Id);
    }

    public override async Task<ProductSetDto> UpdateAsync(Guid id, CreateUpdateProductSetDto input)
    {
        Normalize(input);
        Validate(input);

        // Update scalar fields
        var updated = await base.UpdateAsync(id, input);

        // Sync links
        var q = await _linkRepo.GetQueryableAsync();
        var existing = q.Where(x => x.ProductSetId == id).ToList();
        var existingIds = existing.Select(x => x.CarId).ToHashSet();
        var desiredIds = (input.CompatibleCarIds ?? new List<Guid>()).ToHashSet();

        // Deletes
        foreach (var link in existing.Where(l => !desiredIds.Contains(l.CarId)))
        {
            await _linkRepo.DeleteAsync(link, autoSave: true);
        }

        // Inserts
        foreach (var addId in desiredIds.Where(cid => !existingIds.Contains(cid)))
        {
            await _linkRepo.InsertAsync(
                new ProductSetCar(Guid.NewGuid(), id, addId),
                autoSave: true
            );
        }

        return await GetAsync(id);
    }

    private static void Normalize(CreateUpdateProductSetDto x)
    {
        x.Name = (x.Name ?? string.Empty).Trim();
        x.Description = string.IsNullOrWhiteSpace(x.Description) ? null : x.Description.Trim();
        x.LookVariant = string.IsNullOrWhiteSpace(x.LookVariant) ? null : x.LookVariant.Trim();
        x.Color = string.IsNullOrWhiteSpace(x.Color) ? null : x.Color.Trim();

        // de-duplicate car ids
        x.CompatibleCarIds = (x.CompatibleCarIds ?? new List<Guid>()).Distinct().ToList();
    }

    private static void Validate(CreateUpdateProductSetDto x)
    {
        if (string.IsNullOrWhiteSpace(x.Name))
            throw new UserFriendlyException("Name is required.");

        if (x.SizeInInches <= 0)
            throw new UserFriendlyException("Size (inches) must be greater than 0.");
    }

    public async Task<PagedResultDto<ProductSetDto>> GetListAsync(ProductSetsListInput input)
    {
        var q = await Repository.GetQueryableAsync();

        string? name = string.IsNullOrWhiteSpace(input.Name) ? null : input.Name!.ToLower();

        q = q
            .WhereIf(name != null, x => EF.Functions.Like(x.Name.ToLower(), $"%{name}%"))
            .WhereIf(input.SizeInInches.HasValue, x => x.SizeInInches == input.SizeInInches!.Value);

        var total = await AsyncExecuter.CountAsync(q);

        // Sorting by Name only, support "Name" or "Name DESC" from UI (no Dynamic LINQ)
        var sorting = (input.Sorting ?? string.Empty).Trim();
        var desc = sorting.EndsWith("DESC", StringComparison.OrdinalIgnoreCase);

        var page = (desc ? q.OrderByDescending(x => x.Name) : q.OrderBy(x => x.Name))
            .ThenBy(x => x.Id) // stable secondary sort
            .PageBy(input.SkipCount, input.MaxResultCount);

        var list = await AsyncExecuter.ToListAsync(page);
        var dtos = ObjectMapper.Map<List<ProductSet>, List<ProductSetDto>>(list);

        return new PagedResultDto<ProductSetDto>(total, dtos);
    }

}
