// Displays/IDisplayAppService.cs
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace VuDrive.Displays;

public interface IDisplayAppService :
    ICrudAppService<DisplayDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateDisplayDto>
{ }

public class DisplayAppService :
    CrudAppService<Display, DisplayDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateDisplayDto>,
    IDisplayAppService
{
    public DisplayAppService(IRepository<Display, Guid> repo) : base(repo) { }
}