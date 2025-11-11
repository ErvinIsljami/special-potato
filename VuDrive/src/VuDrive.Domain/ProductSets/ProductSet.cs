using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace VuDrive.ProductSets;

public class ProductSet : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; } = default!;        //required
    public string? Description { get; set; }
    public decimal SizeInInches { get; set; }           //required
    public string? LookVariant { get; set; } = default!;
    public string? Color { get; set; } = default!;
    public bool Cd { get; set; } = false;
    public bool BuiltInDisplay { get; set; } = false;

    // Many-to-many link to Cars
    public virtual ICollection<ProductSetCar> CompatibleCars { get; set; } = new List<ProductSetCar>();

    protected ProductSet() { }
    public ProductSet(Guid id, string name) : base(id) => Name = name;
}
