using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace VuDrive.Data;

/* This is used if database provider does't define
 * IVuDriveDbSchemaMigrator implementation.
 */
public class NullVuDriveDbSchemaMigrator : IVuDriveDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
