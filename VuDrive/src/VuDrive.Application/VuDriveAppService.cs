using VuDrive.Localization;
using Volo.Abp.Application.Services;

namespace VuDrive;

/* Inherit your application services from this class.
 */
public abstract class VuDriveAppService : ApplicationService
{
    protected VuDriveAppService()
    {
        LocalizationResource = typeof(VuDriveResource);
    }
}
