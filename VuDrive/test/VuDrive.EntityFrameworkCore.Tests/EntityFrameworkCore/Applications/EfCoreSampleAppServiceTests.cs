using VuDrive.Samples;
using Xunit;

namespace VuDrive.EntityFrameworkCore.Applications;

[Collection(VuDriveTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<VuDriveEntityFrameworkCoreTestModule>
{

}
