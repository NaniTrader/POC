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
    public class EfCoreEquityOptionSecurityRepository
    : EfCoreRepository<NaniTraderDbContext, EquityOptionSecurity, Guid>,
        IEquityOptionSecurityRepository
    {
        public EfCoreEquityOptionSecurityRepository(IDbContextProvider<NaniTraderDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<EquityOptionSecurity?> FindByParentIdAndNameAsync(Guid parentId, string name)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(equityOptionSecurity => equityOptionSecurity.Name == name && equityOptionSecurity.ParentId == parentId);
        }

        public async Task<List<EquityOptionSecurity>> GetPagedListWithNameFilterAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            string? filter)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .WhereIf(!filter.IsNullOrWhiteSpace(), equityOptionSecurity => equityOptionSecurity.Name.Contains(filter!))
                .OrderBy(sorting)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }
    }
}
