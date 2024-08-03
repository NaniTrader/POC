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
    public class EfCoreIndexOptionSecurityRepository
    : EfCoreRepository<NaniTraderDbContext, IndexOptionSecurity, Guid>,
        IIndexOptionSecurityRepository
    {
        public EfCoreIndexOptionSecurityRepository(IDbContextProvider<NaniTraderDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<IndexOptionSecurity?> FindByParentIdAndNameAsync(Guid parentId, string name)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(indexOptionSecurity => indexOptionSecurity.Name == name && indexOptionSecurity.ParentId == parentId);
        }

        public async Task<List<IndexOptionSecurity>> GetPagedListWithNameFilterAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            string? filter)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .WhereIf(!filter.IsNullOrWhiteSpace(), indexOptionSecurity => indexOptionSecurity.Name.Contains(filter!))
                .OrderBy(sorting)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }
    }
}
