using System.Threading.Tasks;
using VuDrive.Localization;
using VuDrive.Permissions;
using VuDrive.MultiTenancy;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.UI.Navigation;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.Identity.Blazor;

namespace VuDrive.Blazor.Menus;

public class VuDriveMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<VuDriveResource>();
        

        context.Menu.AddItem(new ApplicationMenuItem("VuDrive.Cars", "Cars", url: "/cars", icon: "fa fa-car"));
        context.Menu.AddItem(new ApplicationMenuItem("VuDrive.ProductSets", "Product Sets", url: "/product-sets", icon: "fa fa-cubes"));
        context.Menu.AddItem(new ApplicationMenuItem("VuDrive.Displays", "Displays", url: "/displays", icon: "fa fa-tv"));


        //Administration
        var administration = context.Menu.GetAdministration();
        administration.Order = 6;

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenus.GroupName, 3);

        return Task.CompletedTask;
    }
}
