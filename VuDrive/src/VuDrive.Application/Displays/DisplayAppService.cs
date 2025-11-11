// Displays/IDisplayAppService.cs
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace VuDrive.Displays;

public interface IDisplayAppService :
    ICrudAppService<DisplayDto, Guid, DisplaysListInput, CreateUpdateDisplayDto, CreateUpdateDisplayDto>
{
}

public class DisplayAppService
  : CrudAppService<Display, DisplayDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateDisplayDto, CreateUpdateDisplayDto>,
    IDisplayAppService
{
    public DisplayAppService(IRepository<Display, Guid> repo) : base(repo) { }

    public override async Task<DisplayDto> CreateAsync(CreateUpdateDisplayDto input)
    {
        Normalize(input);
        Validate(input);
        return await base.CreateAsync(input);
    }

    public override async Task<DisplayDto> UpdateAsync(Guid id, CreateUpdateDisplayDto input)
    {
        Normalize(input);
        Validate(input);
        return await base.UpdateAsync(id, input);
    }

    private static void Normalize(CreateUpdateDisplayDto x)
    {
        x.Name = (x.Name ?? string.Empty).Trim();
        x.AndroidVersion = string.IsNullOrWhiteSpace(x.AndroidVersion) ? null : x.AndroidVersion.Trim();
        x.Cpu = string.IsNullOrWhiteSpace(x.Cpu) ? null : x.Cpu.Trim();
        // Memory left as-is (0 allowed), Ram/Size validated below
    }

    private static void Validate(CreateUpdateDisplayDto x)
    {
        if (string.IsNullOrWhiteSpace(x.Name))
            throw new UserFriendlyException("Name is required.");

        if (x.SizeInInches <= 0)
            throw new UserFriendlyException("Size (inches) must be greater than 0.");

        if (x.Ram <= 0)
            throw new UserFriendlyException("RAM must be at least 1 GB.");
    }

    public async Task<PagedResultDto<DisplayDto>> GetListAsync(DisplaysListInput input)
    {
        var q = await Repository.GetQueryableAsync();

        string? name = string.IsNullOrWhiteSpace(input.Name) ? null : input.Name!.ToLower();

        q = q.WhereIf(name != null, x => EF.Functions.Like(x.Name.ToLower(), $"%{name}%"))
            .WhereIf(input.SizeInInches.HasValue, x => x.SizeInInches == input.SizeInInches!.Value)
            .WhereIf(input.Ram.HasValue, x => x.Ram == input.Ram!.Value);

        var total = await AsyncExecuter.CountAsync(q);

        // simple deterministic default sort:
        var page = q
            .OrderBy(x => x.Name)
            .ThenBy(x => x.Ram)
            .PageBy(input.SkipCount, input.MaxResultCount);

        var list = await AsyncExecuter.ToListAsync(page);
        var dtos = ObjectMapper.Map<List<Display>, List<DisplayDto>>(list);

        return new PagedResultDto<DisplayDto>(total, dtos);
    }

}
