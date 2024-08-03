﻿using NaniTrader.Entities.MarketData;
using NaniTrader.Services.MarketData.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace NaniTrader.Services.MarketData
{
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

        public async Task<MarketDataProviderDto> CreateAsync(CreateUpdateMarketDataProviderDto input)
        {
            var marketDataProvider = await _marketDataProviderManager.CreateAsync(
                input.Name,
                input.Description
            );

            await _marketDataProviderRepository.InsertAsync(marketDataProvider);

            return ObjectMapper.Map<MarketDataProvider, MarketDataProviderDto>(marketDataProvider);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _marketDataProviderRepository.DeleteAsync(id);
        }
    }
}
