using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace NaniTrader.Data;

public class NaniTraderEFCoreDbSchemaMigrator : ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public NaniTraderEFCoreDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the NaniTraderDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<NaniTraderDbContext>()
            .Database
            .MigrateAsync();
    }
}
