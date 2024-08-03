using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using NaniTrader.Entities.MarketData;

namespace NaniTrader.Data.MarketData
{
    public class EfCoreMarketDataProviderRepository
    : EfCoreRepository<NaniTraderDbContext, MarketDataProvider, Guid>,
        IMarketDataProviderRepository
    {
        public EfCoreMarketDataProviderRepository(IDbContextProvider<NaniTraderDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<MarketDataProvider?> FindByNameAsync(string name)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(marketDataProvider => marketDataProvider.Name == name);
        }

        public async Task<List<MarketDataProvider>> GetPagedListWithNameFilterAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            string? filter)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .WhereIf(!filter.IsNullOrWhiteSpace(), marketDataProvider => marketDataProvider.Name.Contains(filter!))
                .OrderBy(sorting)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }
    }
}
