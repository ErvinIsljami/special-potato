using Microsoft.Extensions.Localization;
using VuDrive.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace VuDrive.Blazor;

[Dependency(ReplaceServices = true)]
public class VuDriveBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<VuDriveResource> _localizer;

    public VuDriveBrandingProvider(IStringLocalizer<VuDriveResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
