using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NaniTrader.Services.Brokers
{
    public interface IBrokerAppService : IApplicationService
    {
        Task<BrokerDto> GetAsync(Guid id);

        Task<BrokerDto> CreateAsync(CreateUpdateBrokerDto input);

        Task UpdateAsync(Guid id, CreateUpdateBrokerDto input);

        Task<PagedResultDto<BrokerInListDto>> GetPagedListWithNameFilterAsync(BrokerListFilterDto input);

        Task DeleteAsync(Guid id);
    }
}
