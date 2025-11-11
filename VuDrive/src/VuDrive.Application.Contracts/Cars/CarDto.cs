using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace VuDrive.Cars;

public class CarDto : AuditedEntityDto<Guid>
{
    public string Mark { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string? SpecificationModel { get; set; }
    public List<string> YearsBuilt { get; set; } = new();
}

public class CreateUpdateCarDto
{
    [Required, MaxLength(128)]
    [RegularExpression(@".*\S.*", ErrorMessage = "Mark cannot be empty or whitespace.")]
    public string Mark { get; set; } = string.Empty;

    [Required, MaxLength(128)]
    [RegularExpression(@".*\S.*", ErrorMessage = "Model cannot be empty or whitespace.")]
    public string Model { get; set; } = string.Empty;

    public string? SpecificationModel { get; set; } // optional
    public List<string> YearsBuilt { get; set; } = new();
}

public class ImportCarsUploadDto
{
    public string FileName { get; set; } = string.Empty;
    public byte[] Content { get; set; } = Array.Empty<byte>();
}

public class CarsListInput : PagedAndSortedResultRequestDto
{
    public string? Mark { get; set; }
    public string? Model { get; set; }
    public string? Spec { get; set; } // SpecificationModel substring
}