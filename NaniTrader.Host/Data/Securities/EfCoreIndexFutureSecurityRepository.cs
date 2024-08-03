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
    public class EfCoreIndexFutureSecurityRepository
    : EfCoreRepository<NaniTraderDbContext, IndexFutureSecurity, Guid>,
        IIndexFutureSecurityRepository
    {
        public EfCoreIndexFutureSecurityRepository(IDbContextProvider<NaniTraderDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<IndexFutureSecurity?> FindByParentIdAndNameAsync(Guid parentId, string name)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(indexFutureSecurity => indexFutureSecurity.Name == name && indexFutureSecurity.ParentId == parentId);
        }

        public async Task<List<IndexFutureSecurity>> GetPagedListWithNameFilterAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            string? filter)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .WhereIf(!filter.IsNullOrWhiteSpace(), indexFutureSecurity => indexFutureSecurity.Name.Contains(filter!))
                .OrderBy(sorting)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }
    }
}
