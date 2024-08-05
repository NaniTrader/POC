using NaniTrader.Services.Brokers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NaniTrader.Services.Exchanges
{
    public interface IExchangeAppService : IApplicationService
    {
        Task<ExchangeDto> GetAsync(Guid id);

        Task<ExchangeDto> CreateAsync(CreateUpdateExchangeDto input);

        Task UpdateAsync(Guid id, CreateUpdateExchangeDto input);

        Task<PagedResultDto<ExchangeInListDto>> GetPagedListWithNameFilterAsync(ExchangeListFilterDto input);

        Task DeleteAsync(Guid id);
    }
}
