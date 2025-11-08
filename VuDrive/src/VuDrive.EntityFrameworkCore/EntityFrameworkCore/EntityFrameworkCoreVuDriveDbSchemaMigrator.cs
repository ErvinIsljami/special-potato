using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VuDrive.Data;
using Volo.Abp.DependencyInjection;

namespace VuDrive.EntityFrameworkCore;

public class EntityFrameworkCoreVuDriveDbSchemaMigrator
    : IVuDriveDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreVuDriveDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the VuDriveDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<VuDriveDbContext>()
            .Database
            .MigrateAsync();
    }
}
