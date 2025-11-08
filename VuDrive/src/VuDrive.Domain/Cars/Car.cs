using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Entities.Auditing;

namespace VuDrive.Cars;

public class Car : FullAuditedAggregateRoot<Guid>
{
    public string Mark { get; set; } = default!;
    public string Model { get; set; } = default!;
    public string SpecificationModel { get; set; } = default!;

    // List of specific years (e.g. "2012", "2013")
    public List<string> YearsBuilt { get; set; } = new();

    // Back-link for many-to-many (no UI exposure required)
    public virtual ICollection<VuDrive.ProductSets.ProductSetCar> ProductSets { get; set; }
        = new List<VuDrive.ProductSets.ProductSetCar>();

    protected Car() { }

    public Car(Guid id, string mark, string model, string spec, IEnumerable<string>? yearsBuilt = null)
        : base(id)
    {
        Mark = mark;
        Model = model;
        SpecificationModel = spec;
        YearsBuilt = yearsBuilt?.ToList() ?? new List<string>();
    }
}
