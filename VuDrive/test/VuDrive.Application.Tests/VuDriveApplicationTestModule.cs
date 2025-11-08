using Volo.Abp.Modularity;

namespace VuDrive;

[DependsOn(
    typeof(VuDriveApplicationModule),
    typeof(VuDriveDomainTestModule)
)]
public class VuDriveApplicationTestModule : AbpModule
{

}
