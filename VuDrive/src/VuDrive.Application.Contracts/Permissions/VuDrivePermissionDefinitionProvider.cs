using VuDrive.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace VuDrive.Permissions;

public class VuDrivePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(VuDrivePermissions.GroupName);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(VuDrivePermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<VuDriveResource>(name);
    }
}
