using Volo.Abp.Modularity;

namespace VuDrive;

[DependsOn(
    typeof(VuDriveDomainModule),
    typeof(VuDriveTestBaseModule)
)]
public class VuDriveDomainTestModule : AbpModule
{

}
