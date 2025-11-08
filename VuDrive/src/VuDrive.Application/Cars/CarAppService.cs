using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace VuDrive.Cars;

public interface ICarAppService :
    ICrudAppService<CarDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateCarDto>
{ }

public class CarAppService :
    CrudAppService<Car, CarDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateCarDto>,
    ICarAppService
{
    public CarAppService(IRepository<Car, Guid> repo) : base(repo) { }
}
