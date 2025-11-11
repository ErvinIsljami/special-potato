using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using System;
using System.Linq;

namespace VuDrive.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ConnectionStringName("Default")]
public class VuDriveDbContext :
    AbpDbContext<VuDriveDbContext>,
    IIdentityDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */
    public DbSet<VuDrive.Cars.Car> Cars { get; set; } = default!;
    public DbSet<VuDrive.ProductSets.ProductSet> ProductSets { get; set; } = default!;
    public DbSet<VuDrive.ProductSets.ProductSetCar> ProductSetCars { get; set; } = default!;
    public DbSet<VuDrive.Displays.Display> Displays { get; set; } = default!;

    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext 
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext .
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }

    #endregion

    public VuDriveDbContext(DbContextOptions<VuDriveDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureBlobStoring();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(VuDriveConsts.DbTablePrefix + "YourEntities", VuDriveConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});

        builder.Entity<VuDrive.Displays.Display>(b =>
        {
            b.ToTable(VuDriveConsts.DbTablePrefix + "Displays", VuDriveConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired();
            b.Property(x => x.SizeInInches).IsRequired();
            b.Property(x => x.Ram).IsRequired();
            b.Property(x => x.AndroidVersion).IsRequired(false);
            b.Property(x => x.Cpu).IsRequired(false);
            b.Property(x => x.Memory).IsRequired(false);
        });

        builder.Entity<VuDrive.Cars.Car>(b =>
        {
            b.ToTable(VuDriveConsts.DbTablePrefix + "Cars", VuDriveConsts.DbSchema);
            b.ConfigureByConvention();


            // Store list<string> as a comma-separated string (simple & portable)
            b.Property(x => x.YearsBuilt)
             .HasConversion(
                v => string.Join(",", v ?? new()),
                v => (v ?? string.Empty)
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .ToList()
             )
             .HasMaxLength(512);

            b.Property(x => x.Mark).IsRequired();
            b.Property(x => x.Model).IsRequired();

            b.Property(x => x.SpecificationModel).IsRequired(false);
        });

        builder.Entity<VuDrive.ProductSets.ProductSet>(b =>
        {
            b.ToTable(VuDriveConsts.DbTablePrefix + "ProductSets", VuDriveConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired();
            b.Property(x => x.SizeInInches).IsRequired();
            b.Property(x => x.Description).IsRequired(false);
            b.Property(x => x.LookVariant).IsRequired(false);
            b.Property(x => x.Color).IsRequired(false);
        });

        builder.Entity<VuDrive.ProductSets.ProductSetCar>(b =>
        {
            b.ToTable(VuDriveConsts.DbTablePrefix + "ProductSetCars", VuDriveConsts.DbSchema);
            b.ConfigureByConvention();

            // Unique link per (ProductSet, Car)
            b.HasIndex(x => new { x.ProductSetId, x.CarId }).IsUnique();

            b.HasOne(x => x.ProductSet)
             .WithMany(ps => ps.CompatibleCars)
             .HasForeignKey(x => x.ProductSetId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.Car)
             .WithMany(c => c.ProductSets)
             .HasForeignKey(x => x.CarId)
             .OnDelete(DeleteBehavior.Cascade);
        });



    }
}
