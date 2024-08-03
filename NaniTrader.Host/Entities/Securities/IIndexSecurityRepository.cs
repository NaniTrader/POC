using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NaniTrader.Entities.Securities
{
    public interface IIndexSecurityRepository : IRepository<IndexSecurity, Guid>
    {
        Task<IndexSecurity?> FindByParentIdAndNameAsync(Guid parentId, string name);

        Task<List<IndexSecurity>> GetPagedListWithNameFilterAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            string? filter
        );
    }
}
