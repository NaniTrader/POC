using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NaniTrader.Entities.Securities
{
    public interface IEquitySecurityRepository : IRepository<EquitySecurity, Guid>
    {
        Task<EquitySecurity?> FindByParentIdAndNameAsync(Guid parentId, string name);

        Task<List<EquitySecurity>> GetPagedListWithNameFilterAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            string? filter
        );
    }
}
