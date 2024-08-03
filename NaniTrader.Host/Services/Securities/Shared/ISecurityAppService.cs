using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NaniTrader.Services.Securities.Shared
{
    public interface ISecurityAppService : IApplicationService
    {
        Task<EquitySecurityDto> GetEquitySecurityAsync(Guid id);

        Task<EquitySecurityDto> CreateEquitySecurityAsync(CreateUpdateEquitySecurityDto input);

        Task<PagedResultDto<EquitySecurityInListDto>> GetEquitySecurityPagedListWithNameFilterAsync(EquitySecurityListFilterDto input);

        Task DeleteEquitySecurityAsync(Guid id);
    }
}
