using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NaniTrader.Entities.Exchanges
{
    public interface IExchangeRepository : IRepository<Exchange, Guid>
    {
        Task<Exchange?> FindByNameAsync(string name);

        Task<List<Exchange>> GetPagedListWithNameFilterAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            string? filter
        );
    }
}
