using VuDrive.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace VuDrive.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class VuDriveController : AbpControllerBase
{
    protected VuDriveController()
    {
        LocalizationResource = typeof(VuDriveResource);
    }
}
