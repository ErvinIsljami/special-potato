// Displays/DisplayDto.cs
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace VuDrive.Displays;

public class DisplayDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; } = string.Empty;
    public decimal SizeInInches { get; set; }
    public string? AndroidVersion { get; set; }
    public int Ram { get; set; }
    public int? Memory { get; set; }
    public string? Cpu { get; set; }
}

public class CreateUpdateDisplayDto
{
    [Required, MaxLength(128)]
    [RegularExpression(@".*\S.*", ErrorMessage = "Name cannot be empty or whitespace.")]
    public string Name { get; set; } = string.Empty;

    [Range(0.1, 20, ErrorMessage = "Size (inches) must be greater than 0.")]
    public decimal SizeInInches { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "RAM must be at least 1 GB.")]
    public int Ram { get; set; }

    // Optional
    [MaxLength(32)]
    public string? AndroidVersion { get; set; }

    [Range(0, int.MaxValue)]
    public int? Memory { get; set; } // 0 means “unspecified/unknown” if you like

    [MaxLength(128)]
    public string? Cpu { get; set; }
}

public class DisplaysListInput : PagedAndSortedResultRequestDto
{
    public string? Name { get; set; }           // contains, case-insensitive
    public decimal? SizeInInches { get; set; }  // exact
    public int? Ram { get; set; }               // exact
}
