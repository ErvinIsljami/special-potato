using Xunit;

namespace VuDrive.EntityFrameworkCore;

[CollectionDefinition(VuDriveTestConsts.CollectionDefinitionName)]
public class VuDriveEntityFrameworkCoreCollection : ICollectionFixture<VuDriveEntityFrameworkCoreFixture>
{

}
