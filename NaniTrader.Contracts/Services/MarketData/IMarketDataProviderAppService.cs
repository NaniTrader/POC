using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NaniTrader.Services.MarketData
{
    public interface IMarketDataProviderAppService : IApplicationService
    {
        Task<MarketDataProviderDto> GetAsync(Guid id);

        Task<MarketDataProviderDto> CreateAsync(CreateUpdateMarketDataProviderDto input);

        Task<PagedResultDto<MarketDataProviderInListDto>> GetPagedListWithNameFilterAsync(MarketDataProviderListFilterDto input);

        Task DeleteAsync(Guid id);
    }
}
