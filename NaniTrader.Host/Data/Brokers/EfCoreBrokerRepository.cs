using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using NaniTrader.Entities.Brokers;

namespace NaniTrader.Data.Brokers
{
    public class EfCoreBrokerRepository
    : EfCoreRepository<NaniTraderDbContext, Broker, Guid>,
        IBrokerRepository
    {
        public EfCoreBrokerRepository(IDbContextProvider<NaniTraderDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Broker?> FindByNameAsync(string name)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(broker => broker.Name == name);
        }

        public async Task<List<Broker>> GetPagedListWithNameFilterAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            string? filter)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .WhereIf(!filter.IsNullOrWhiteSpace(), broker => broker.Name.Contains(filter!))
                .OrderBy(sorting)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }
    }
}
