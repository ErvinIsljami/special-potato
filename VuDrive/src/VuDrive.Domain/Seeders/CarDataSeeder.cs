// Seeders/CarDataSeeder.cs
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Data;
using VuDrive.Cars;
using CsvHelper;
using CsvHelper.Configuration;

public class CarCsvRecord
{
    public string Mark { get; set; } = default!;
    public string Model { get; set; } = default!;
    public string SpecificationModel { get; set; } = default!;
    public string YearsBuilt { get; set; } = default!;
}

public class CarDataSeeder : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Car, Guid> _carRepo;

    public CarDataSeeder(IRepository<Car, Guid> carRepo)
        => _carRepo = carRepo;

    public async Task SeedAsync(DataSeedContext context)
    {
        //if (await _carRepo.GetCountAsync() > 0) return; // already seeded

        var path = Path.Combine(AppContext.BaseDirectory, "../seed_cars_serbia.csv");
        if (!File.Exists(path)) return;

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ",",
            Encoding = System.Text.Encoding.UTF8
        };

        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, config);

        var records = csv.GetRecords<CarCsvRecord>().ToList();
        foreach (var r in records)
        {
            var years = (r.YearsBuilt ?? "")
                .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();

            var car = new Car(Guid.NewGuid(), r.Mark, r.Model, r.SpecificationModel, years);
            await _carRepo.InsertAsync(car, autoSave: true);
        }
    }
}
