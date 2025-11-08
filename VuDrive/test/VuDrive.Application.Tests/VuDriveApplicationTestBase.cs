using Volo.Abp.Modularity;

namespace VuDrive;

public abstract class VuDriveApplicationTestBase<TStartupModule> : VuDriveTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
