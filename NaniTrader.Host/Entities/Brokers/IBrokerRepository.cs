using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NaniTrader.Entities.Brokers
{
    public interface IBrokerRepository : IRepository<Broker, Guid>
    {
        Task<Broker?> FindByNameAsync(string name);

        Task<List<Broker>> GetPagedListWithNameFilterAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            string? filter
        );
    }
}
