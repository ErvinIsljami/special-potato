using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace VuDrive.ProductSets;

public class ProductSetDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal SizeInInches { get; set; }
    public string? LookVariant { get; set; }
    public string? Color { get; set; }
    public bool Cd { get; set; }
    public bool BuiltInDisplay { get; set; }

    // IDs of selected Cars
    public List<Guid> CompatibleCarIds { get; set; } = new();
}

public class CreateUpdateProductSetDto
{
    [Required, MaxLength(128)]
    [RegularExpression(@".*\S.*", ErrorMessage = "Name cannot be empty or whitespace.")]
    public string Name { get; set; } = string.Empty;

    [Range(0.1, 200, ErrorMessage = "Size (inches) must be greater than 0.")]
    public decimal SizeInInches { get; set; }

    [MaxLength(2048)]
    public string? Description { get; set; }

    [MaxLength(64)]
    public string? LookVariant { get; set; }

    [MaxLength(32)]
    public string? Color { get; set; }

    public bool Cd { get; set; }
    public bool BuiltInDisplay { get; set; }

    // IDs of selected Cars
    public List<Guid> CompatibleCarIds { get; set; } = new();
}

public class ProductSetsListInput : PagedAndSortedResultRequestDto
{
    public string? Name { get; set; }             // contains (case-insensitive)
    public decimal? SizeInInches { get; set; }    // exact
}
