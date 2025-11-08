using VuDrive.Samples;
using Xunit;

namespace VuDrive.EntityFrameworkCore.Domains;

[Collection(VuDriveTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<VuDriveEntityFrameworkCoreTestModule>
{

}
