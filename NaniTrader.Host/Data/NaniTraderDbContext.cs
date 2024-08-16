using Microsoft.EntityFrameworkCore;
using NaniTrader.Entities.Brokers;
using NaniTrader.Entities.Exchanges;
using NaniTrader.Entities.MarketData;
using NaniTrader.Entities.Misc;
using NaniTrader.Entities.Securities;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace NaniTrader.Data;

public class NaniTraderDbContext : AbpDbContext<NaniTraderDbContext>
{
    public DbSet<Exchange> Exchanges { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Broker> Brokers { get; set; }
    public DbSet<MarketDataProvider> MarketDataProviders { get; set; }
    public DbSet<EquitySecurity> EquitySecurities { get; set; }
    public DbSet<EquityFutureSecurity> EquityFutureSecurities { get; set; }
    public DbSet<EquityOptionSecurity> EquityOptionSecurities { get; set; }
    public DbSet<IndexSecurity> IndexSecurities { get; set; }
    public DbSet<IndexFutureSecurity> IndexFutureSecurities { get; set; }
    public DbSet<IndexOptionSecurity> IndexOptionSecurities { get; set; }

    public NaniTraderDbContext(DbContextOptions<NaniTraderDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own entities here */
        builder.ConfigureNaniTraderBrokers(this);
        builder.ConfigureNaniTraderExchanges(this);
        builder.ConfigureNaniTraderMarketData(this);
        builder.ConfigureNaniTraderMisc(this);
        builder.ConfigureNaniTraderSecurities(this);
    }
}
