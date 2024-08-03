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
    public class EfCoreIndexSecurityRepository
    : EfCoreRepository<NaniTraderDbContext, IndexSecurity, Guid>,
        IIndexSecurityRepository
    {
        public EfCoreIndexSecurityRepository(IDbContextProvider<NaniTraderDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<IndexSecurity?> FindByParentIdAndNameAsync(Guid parentId, string name)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(indexSecurity => indexSecurity.Name == name && indexSecurity.ParentId == parentId);
        }

        public async Task<List<IndexSecurity>> GetPagedListWithNameFilterAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            string? filter)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .WhereIf(!filter.IsNullOrWhiteSpace(), indexSecurity => indexSecurity.Name.Contains(filter!))
                .OrderBy(sorting)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }
    }
}
