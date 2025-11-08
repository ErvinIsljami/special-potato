using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace VuDrive.ProductSets;

public class ProductSetDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal SizeInInches { get; set; }
    public string LookVariant { get; set; } = default!;
    public string Color { get; set; } = default!;
    public bool Cd { get; set; }
    public bool BuiltInDisplay { get; set; }

    // IDs of selected Cars
    public List<Guid> CompatibleCarIds { get; set; } = new();
}

public class CreateUpdateProductSetDto
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal SizeInInches { get; set; }
    public string LookVariant { get; set; } = default!;
    public string Color { get; set; } = default!;
    public bool Cd { get; set; }
    public bool BuiltInDisplay { get; set; }

    // IDs of selected Cars
    public List<Guid> CompatibleCarIds { get; set; } = new();
}
