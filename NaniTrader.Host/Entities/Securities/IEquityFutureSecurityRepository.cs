using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NaniTrader.Entities.Securities
{
    public interface IEquityFutureSecurityRepository : IRepository<EquityFutureSecurity, Guid>
    {
        Task<EquityFutureSecurity?> FindByParentIdAndNameAsync(Guid parentId, string name);

        Task<List<EquityFutureSecurity>> GetPagedListWithNameFilterAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            string? filter
        );
    }
}
