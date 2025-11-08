using VuDrive.Localization;
using Volo.Abp.AspNetCore.Components;

namespace VuDrive.Blazor;

public abstract class VuDriveComponentBase : AbpComponentBase
{
    protected VuDriveComponentBase()
    {
        LocalizationResource = typeof(VuDriveResource);
    }
}
