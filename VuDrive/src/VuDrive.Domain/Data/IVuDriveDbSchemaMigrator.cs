using System.Threading.Tasks;

namespace VuDrive.Data;

public interface IVuDriveDbSchemaMigrator
{
    Task MigrateAsync();
}
