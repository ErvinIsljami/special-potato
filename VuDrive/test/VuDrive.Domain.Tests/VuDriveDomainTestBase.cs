using Volo.Abp.Modularity;

namespace VuDrive;

/* Inherit from this class for your domain layer tests. */
public abstract class VuDriveDomainTestBase<TStartupModule> : VuDriveTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
