using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NaniTrader.Entities.Securities
{
    public interface ISecurityRepository : IRepository<SecurityBase, Guid>
    {
        Task<SecurityBase?> FindByParentIdAndNameAsync(Guid parentId, string name);

        Task<List<SecurityBase>> GetPagedListWithNameFilterAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            string? filter
        );
    }
}
