using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using NaniTrader.Entities.Securities;

namespace NaniTrader.Data.Exchanges
{
    public class EfCoreEquityFutureSecurityRepository
    : EfCoreRepository<NaniTraderDbContext, EquityFutureSecurity, Guid>,
        IEquityFutureSecurityRepository
    {
        public EfCoreEquityFutureSecurityRepository(IDbContextProvider<NaniTraderDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<EquityFutureSecurity?> FindByParentIdAndNameAsync(Guid parentId, string name)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(equityFutureSecurity => equityFutureSecurity.Name == name && equityFutureSecurity.ParentId == parentId);
        }

        public async Task<List<EquityFutureSecurity>> GetPagedListWithNameFilterAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            string? filter)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .WhereIf(!filter.IsNullOrWhiteSpace(), equityFutureSecurity => equityFutureSecurity.Name.Contains(filter!))
                .OrderBy(sorting)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }
    }
}
