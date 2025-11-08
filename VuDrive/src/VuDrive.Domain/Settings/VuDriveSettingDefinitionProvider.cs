using Volo.Abp.Settings;

namespace VuDrive.Settings;

public class VuDriveSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(VuDriveSettings.MySetting1));
    }
}
