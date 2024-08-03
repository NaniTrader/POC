using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NaniTrader.Entities.Securities
{
    public interface IEquityOptionSecurityRepository : IRepository<EquityOptionSecurity, Guid>
    {
        Task<EquityOptionSecurity?> FindByParentIdAndNameAsync(Guid parentId, string name);

        Task<List<EquityOptionSecurity>> GetPagedListWithNameFilterAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            string? filter
        );
    }
}
