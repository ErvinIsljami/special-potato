using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace VuDrive.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class VuDriveDbContextFactory : IDesignTimeDbContextFactory<VuDriveDbContext>
{
    public VuDriveDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        VuDriveEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<VuDriveDbContext>()
            .UseSqlite(configuration.GetConnectionString("Default"));
        
        return new VuDriveDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../VuDrive.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
