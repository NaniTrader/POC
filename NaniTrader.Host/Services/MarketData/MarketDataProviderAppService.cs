using Microsoft.AspNetCore.Authorization;
using NaniTrader.Entities.Exchanges;
using NaniTrader.Entities.MarketData;
using NaniTrader.Services.Exchanges;
using NaniTrader.Services.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace NaniTrader.Services.MarketData
{
    [Authorize(NaniTraderPermissions.MarketDataProviders.Default)]
    public class MarketDataProviderAppService : NaniTraderAppService, IMarketDataProviderAppService
    {
        private readonly IMarketDataProviderRepository _marketDataProviderRepository;
        private readonly MarketDataProviderManager _marketDataProviderManager;

        public MarketDataProviderAppService(
            IMarketDataProviderRepository marketDataProviderRepository,
            MarketDataProviderManager marketDataProviderManager)
        {
            _marketDataProviderRepository = marketDataProviderRepository;
            _marketDataProviderManager = marketDataProviderManager;
        }

        public async Task<MarketDataProviderDto> GetAsync(Guid id)
        {
            var marketDataProvider = await _marketDataProviderRepository.GetAsync(id);
            return ObjectMapper.Map<MarketDataProvider, MarketDataProviderDto>(marketDataProvider);
        }

        public async Task<PagedResultDto<MarketDataProviderInListDto>> GetPagedListWithNameFilterAsync(MarketDataProviderListFilterDto input)
        {
            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(MarketDataProvider.Name);
            }

            var marketDataProviders = await _marketDataProviderRepository.GetPagedListWithNameFilterAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting,
                input.Name
            );

            var totalCount = input.Name == null
                ? await _marketDataProviderRepository.CountAsync()
                : await _marketDataProviderRepository.CountAsync(
                    exchange => exchange.Name.Contains(input.Name));

            return new PagedResultDto<MarketDataProviderInListDto>(
                totalCount,
                ObjectMapper.Map<List<MarketDataProvider>, List<MarketDataProviderInListDto>>(marketDataProviders)
            );
        }

        [Authorize(NaniTraderPermissions.MarketDataProviders.Create)]
        public async Task<MarketDataProviderDto> CreateAsync(CreateUpdateMarketDataProviderDto input)
        {
            var marketDataProvider = await _marketDataProviderManager.CreateAsync(
                input.Name,
                input.Description
            );

            await _marketDataProviderRepository.InsertAsync(marketDataProvider);

            return ObjectMapper.Map<MarketDataProvider, MarketDataProviderDto>(marketDataProvider);
        }

        [Authorize(NaniTraderPermissions.MarketDataProviders.Edit)]
        public async Task UpdateAsync(Guid id, CreateUpdateMarketDataProviderDto input)
        {
            var broker = await _marketDataProviderRepository.GetAsync(id);

            if (broker.Name != input.Name)
            {
                await _marketDataProviderManager.UpdateNameAsync(broker, input.Name);
            }

            await _marketDataProviderRepository.UpdateAsync(broker);
        }

        [Authorize(NaniTraderPermissions.MarketDataProviders.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _marketDataProviderRepository.DeleteAsync(id);
        }
    }
}
