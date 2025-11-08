using System;
using Volo.Abp.Domain.Entities;

namespace VuDrive.ProductSets;

public class ProductSetCar : Entity<Guid> // <-- give it a primary key
{
    public Guid ProductSetId { get; set; }
    public virtual ProductSet ProductSet { get; set; } = default!;

    public Guid CarId { get; set; }
    public virtual VuDrive.Cars.Car Car { get; set; } = default!;

    // parameterless ctor for EF
    protected ProductSetCar() { }

    public ProductSetCar(Guid id, Guid productSetId, Guid carId)
        : base(id)
    {
        ProductSetId = productSetId;
        CarId = carId;
    }
}
