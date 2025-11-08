using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace VuDrive.Cars;

public class CarDto : AuditedEntityDto<Guid>
{
    public string Mark { get; set; } = default!;
    public string Model { get; set; } = default!;
    public string SpecificationModel { get; set; } = default!;
    public List<string> YearsBuilt { get; set; } = new();
}

public class CreateUpdateCarDto
{
    public string Mark { get; set; } = default!;
    public string Model { get; set; } = default!;
    public string SpecificationModel { get; set; } = default!;
    public List<string> YearsBuilt { get; set; } = new();
}
