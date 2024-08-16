using Microsoft.EntityFrameworkCore;
using NaniTrader.Entities;
using NaniTrader.Entities.Brokers;
using NaniTrader.Entities.Exchanges;
using NaniTrader.Entities.MarketData;
using NaniTrader.Entities.Misc;
using NaniTrader.Entities.Securities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NaniTrader.Data
{
    internal static class NaniTraderDbContextModelCreatingExtensions
    {
        public static void ConfigureNaniTraderMisc(this ModelBuilder builder, NaniTraderDbContext naniTraderDbContext)
        {
            builder.Entity<Country>(b =>
            {
                b.ToTable(NaniTraderConsts.DbTablePrefix + nameof(naniTraderDbContext.Countries), NaniTraderConsts.DbSchemaMisc);
                b.ConfigureByConvention(); //auto configure for the base class props
                b.Property(x => x.Name).IsRequired().HasMaxLength(CountryConsts.MaxNameLength);
            });
        }

        public static void ConfigureNaniTraderExchanges(this ModelBuilder builder, NaniTraderDbContext naniTraderDbContext)
        {
            builder.Entity<Exchange>(b =>
            {
                b.ToTable(NaniTraderConsts.DbTablePrefix + nameof(naniTraderDbContext.Exchanges), NaniTraderConsts.DbSchemaExch);
                b.ConfigureByConvention(); //auto configure for the base class props
                b.Property(x => x.Name).IsRequired().HasMaxLength(ExchangeConsts.MaxNameLength);
                b.Property(x => x.Description).IsRequired().HasMaxLength(ExchangeConsts.MaxDescriptionLength);
            });
        }

        public static void ConfigureNaniTraderBrokers(this ModelBuilder builder, NaniTraderDbContext naniTraderDbContext)
        {
            builder.Entity<Broker>(b =>
            {
                b.ToTable(NaniTraderConsts.DbTablePrefix + nameof(naniTraderDbContext.Brokers), NaniTraderConsts.DbSchemaBrkr);
                b.ConfigureByConvention(); //auto configure for the base class props
                b.Property(x => x.Name).IsRequired().HasMaxLength(BrokerConsts.MaxNameLength);
                b.Property(x => x.Description).IsRequired().HasMaxLength(BrokerConsts.MaxDescriptionLength);
            });
        }

        public static void ConfigureNaniTraderMarketData(this ModelBuilder builder, NaniTraderDbContext naniTraderDbContext)
        {
            builder.Entity<MarketDataProvider>(b =>
            {
                b.ToTable(NaniTraderConsts.DbTablePrefix + nameof(naniTraderDbContext.MarketDataProviders), NaniTraderConsts.DbSchemaMD);
                b.ConfigureByConvention(); //auto configure for the base class props
                b.Property(x => x.Name).IsRequired().HasMaxLength(MarketDataProviderConsts.MaxNameLength);
                b.Property(x => x.Description).IsRequired().HasMaxLength(MarketDataProviderConsts.MaxDescriptionLength);
            });
        }

        public static void ConfigureNaniTraderSecurities(this ModelBuilder builder, NaniTraderDbContext naniTraderDbContext)
        {
            builder.Entity<EquitySecurity>(b =>
            {
                b.ToTable(NaniTraderConsts.DbTablePrefix + nameof(naniTraderDbContext.EquitySecurities), NaniTraderConsts.DbSchemaSecr);
                b.ConfigureByConvention(); //auto configure for the base class props
                b.Property(x => x.Name).IsRequired().HasMaxLength(SecurityConsts.MaxNameLength);
                b.Property(x => x.Description).IsRequired().HasMaxLength(SecurityConsts.MaxDescriptionLength);
            });

            builder.Entity<EquityFutureSecurity>(b =>
            {
                b.ToTable(NaniTraderConsts.DbTablePrefix + nameof(naniTraderDbContext.EquityFutureSecurities), NaniTraderConsts.DbSchemaSecr);
                b.ConfigureByConvention(); //auto configure for the base class props
                b.Property(x => x.Name).IsRequired().HasMaxLength(SecurityConsts.MaxNameLength);
                b.Property(x => x.Description).IsRequired().HasMaxLength(SecurityConsts.MaxDescriptionLength);
            });

            builder.Entity<EquityOptionSecurity>(b =>
            {
                b.ToTable(NaniTraderConsts.DbTablePrefix + nameof(naniTraderDbContext.EquityOptionSecurities), NaniTraderConsts.DbSchemaSecr);
                b.ConfigureByConvention(); //auto configure for the base class props
                b.Property(x => x.Name).IsRequired().HasMaxLength(SecurityConsts.MaxNameLength);
                b.Property(x => x.Description).IsRequired().HasMaxLength(SecurityConsts.MaxDescriptionLength);
            });

            builder.Entity<IndexSecurity>(b =>
            {
                b.ToTable(NaniTraderConsts.DbTablePrefix + nameof(naniTraderDbContext.IndexSecurities), NaniTraderConsts.DbSchemaSecr);
                b.ConfigureByConvention(); //auto configure for the base class props
                b.Property(x => x.Name).IsRequired().HasMaxLength(SecurityConsts.MaxNameLength);
                b.Property(x => x.Description).IsRequired().HasMaxLength(SecurityConsts.MaxDescriptionLength);
            });

            builder.Entity<IndexFutureSecurity>(b =>
            {
                b.ToTable(NaniTraderConsts.DbTablePrefix + nameof(naniTraderDbContext.IndexFutureSecurities), NaniTraderConsts.DbSchemaSecr);
                b.ConfigureByConvention(); //auto configure for the base class props
                b.Property(x => x.Name).IsRequired().HasMaxLength(SecurityConsts.MaxNameLength);
                b.Property(x => x.Description).IsRequired().HasMaxLength(SecurityConsts.MaxDescriptionLength);
            });

            builder.Entity<IndexOptionSecurity>(b =>
            {
                b.ToTable(NaniTraderConsts.DbTablePrefix + nameof(naniTraderDbContext.IndexOptionSecurities), NaniTraderConsts.DbSchemaSecr);
                b.ConfigureByConvention(); //auto configure for the base class props
                b.Property(x => x.Name).IsRequired().HasMaxLength(SecurityConsts.MaxNameLength);
                b.Property(x => x.Description).IsRequired().HasMaxLength(SecurityConsts.MaxDescriptionLength);
            });
        }

        public static void ConfigureNaniTraderNavigations(this ModelBuilder builder)
        {
        }
    }
}
