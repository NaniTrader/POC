﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NaniTrader.Services.Brokers.Shared
{
    public interface IBrokerAppService : IApplicationService
    {
        Task<BrokerDto> GetAsync(Guid id);

        Task<BrokerDto> CreateAsync(CreateUpdateBrokerDto input);

        Task<PagedResultDto<BrokerInListDto>> GetPagedListWithNameFilterAsync(BrokerListFilterDto input);

        Task DeleteAsync(Guid id);
    }
}