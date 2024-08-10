using NaniTrader.Services.MarketData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NaniTrader.Services.Securities
{
    public interface ISecurityAppService : IApplicationService
    {
        Task<EquitySecurityDto> GetEquitySecurityAsync(Guid id);

        Task<EquitySecurityDto> CreateEquitySecurityAsync(CreateUpdateEquitySecurityDto input);

        Task UpdateEquitySecurityAsync(Guid id, CreateUpdateEquitySecurityDto input);

        Task<PagedResultDto<EquitySecurityInListDto>> GetEquitySecurityPagedListWithNameFilterAsync(EquitySecurityListFilterDto input);

        Task DeleteEquitySecurityAsync(Guid id);

        Task<EquityFutureSecurityDto> GetEquityFutureSecurityAsync(Guid id);

        Task<EquityFutureSecurityDto> CreateEquityFutureSecurityAsync(CreateUpdateEquityFutureSecurityDto input);

        Task UpdateEquityFutureSecurityAsync(Guid id, CreateUpdateEquityFutureSecurityDto input);

        Task<PagedResultDto<EquityFutureSecurityInListDto>> GetEquityFutureSecurityPagedListWithNameFilterAsync(EquityFutureSecurityListFilterDto input);

        Task DeleteEquityFutureSecurityAsync(Guid id);

        Task<EquityOptionSecurityDto> GetEquityOptionSecurityAsync(Guid id);

        Task<EquityOptionSecurityDto> CreateEquityOptionSecurityAsync(CreateUpdateEquityOptionSecurityDto input);

        Task UpdateEquityOptionSecurityAsync(Guid id, CreateUpdateEquityOptionSecurityDto input);

        Task<PagedResultDto<EquityOptionSecurityInListDto>> GetEquityOptionSecurityPagedListWithNameFilterAsync(EquityOptionSecurityListFilterDto input);

        Task DeleteEquityOptionSecurityAsync(Guid id);

        Task<IndexSecurityDto> GetIndexSecurityAsync(Guid id);

        Task<IndexSecurityDto> CreateIndexSecurityAsync(CreateUpdateIndexSecurityDto input);

        Task UpdateIndexSecurityAsync(Guid id, CreateUpdateIndexSecurityDto input);

        Task<PagedResultDto<IndexSecurityInListDto>> GetIndexSecurityPagedListWithNameFilterAsync(IndexSecurityListFilterDto input);

        Task DeleteIndexSecurityAsync(Guid id);

        Task<IndexFutureSecurityDto> GetIndexFutureSecurityAsync(Guid id);

        Task<IndexFutureSecurityDto> CreateIndexFutureSecurityAsync(CreateUpdateIndexFutureSecurityDto input);

        Task UpdateIndexFutureSecurityAsync(Guid id, CreateUpdateIndexFutureSecurityDto input);

        Task<PagedResultDto<IndexFutureSecurityInListDto>> GetIndexFutureSecurityPagedListWithNameFilterAsync(IndexFutureSecurityListFilterDto input);

        Task DeleteIndexFutureSecurityAsync(Guid id);

        Task<IndexOptionSecurityDto> GetIndexOptionSecurityAsync(Guid id);

        Task<IndexOptionSecurityDto> CreateIndexOptionSecurityAsync(CreateUpdateIndexOptionSecurityDto input);

        Task UpdateIndexOptionSecurityAsync(Guid id, CreateUpdateIndexOptionSecurityDto input);

        Task<PagedResultDto<IndexOptionSecurityInListDto>> GetIndexOptionSecurityPagedListWithNameFilterAsync(IndexOptionSecurityListFilterDto input);

        Task DeleteIndexOptionSecurityAsync(Guid id);
    }
}
