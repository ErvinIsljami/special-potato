using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace VuDrive.ProductSets;

public interface IProductSetAppService :
    ICrudAppService<ProductSetDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateProductSetDto>
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
}
