using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NaniTrader.Services.Exchanges.Shared
{
    public interface IExchangeAppService : IApplicationService
    {
        Task<ExchangeDto> GetAsync(Ulid id);

        Task<ExchangeDto> CreateAsync(CreateUpdateExchangeDto input);

        Task<PagedResultDto<ExchangeInListDto>> GetPagedListWithNameFilterAsync(ExchangeListFilterDto input);

        Task DeleteAsync(Ulid id);
    }
}
