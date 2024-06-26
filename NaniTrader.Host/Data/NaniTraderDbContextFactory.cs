using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NaniTrader.Data;

public class NaniTraderDbContextFactory : IDesignTimeDbContextFactory<NaniTraderDbContext>
{
    public NaniTraderDbContext CreateDbContext(string[] args)
    {

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<NaniTraderDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new NaniTraderDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
