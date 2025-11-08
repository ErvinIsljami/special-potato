using VuDrive.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace VuDrive.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(VuDriveEntityFrameworkCoreModule),
    typeof(VuDriveApplicationContractsModule)
)]
public class VuDriveDbMigratorModule : AbpModule
{
}
