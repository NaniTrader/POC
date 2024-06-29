using Microsoft.EntityFrameworkCore;
using NaniTrader.Entities;
using NaniTrader.Entities.Exchanges;
using NaniTrader.Entities.Exchanges.Shared;
using NaniTrader.Entities.Misc;
using NaniTrader.Entities.Misc.Shared;
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
        public static void ConfigureNaniTrader(this ModelBuilder builder, NaniTraderDbContext naniTraderDbContext)
        {
            builder.Entity<Country>(b =>
            {
                b.ToTable(NaniTraderConsts.DbTablePrefix + nameof(naniTraderDbContext.Countries), NaniTraderConsts.DbSchema);
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
        public static void ConfigureNaniTraderNavigations(this ModelBuilder builder)
        {
        }
    }
}
