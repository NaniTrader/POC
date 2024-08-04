using Microsoft.AspNetCore.Authorization;
using NaniTrader.Entities.Brokers;
using NaniTrader.Services.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace NaniTrader.Services.Brokers
{
    [Authorize(NaniTraderPermissions.Brokers.Default)]
    public class BrokerAppService : NaniTraderAppService, IBrokerAppService
    {
        private readonly IBrokerRepository _brokerRepository;
        private readonly BrokerManager _brokerManager;

        public BrokerAppService(
            IBrokerRepository brokerRepository,
            BrokerManager brokerManager)
        {
            _brokerRepository = brokerRepository;
            _brokerManager = brokerManager;
        }

        public async Task<BrokerDto> GetAsync(Guid id)
        {
            var broker = await _brokerRepository.GetAsync(id);
            return ObjectMapper.Map<Broker, BrokerDto>(broker);
        }

        public async Task<PagedResultDto<BrokerInListDto>> GetPagedListWithNameFilterAsync(BrokerListFilterDto input)
        {
            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(Broker.Name);
            }

            var brokers = await _brokerRepository.GetPagedListWithNameFilterAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting,
                input.Name
            );

            var totalCount = input.Name == null
                ? await _brokerRepository.CountAsync()
                : await _brokerRepository.CountAsync(
                    broker => broker.Name.Contains(input.Name));

            return new PagedResultDto<BrokerInListDto>(
                totalCount,
                ObjectMapper.Map<List<Broker>, List<BrokerInListDto>>(brokers)
            );
        }

        [Authorize(NaniTraderPermissions.Brokers.Create)]
        public async Task<BrokerDto> CreateAsync(CreateUpdateBrokerDto input)
        {
            var broker = await _brokerManager.CreateAsync(
                input.Name,
                input.Description
            );

            await _brokerRepository.InsertAsync(broker);

            return ObjectMapper.Map<Broker, BrokerDto>(broker);
        }

        [Authorize(NaniTraderPermissions.Brokers.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _brokerRepository.DeleteAsync(id);
        }
    }
}
