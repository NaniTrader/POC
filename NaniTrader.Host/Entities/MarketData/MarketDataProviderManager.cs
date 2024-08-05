using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;
using Volo.Abp;
using NaniTrader.Entities.Brokers;
using NaniTrader.Entities.Exchanges;

namespace NaniTrader.Entities.MarketData
{
    public class MarketDataProviderManager : DomainService
    {
        private readonly IMarketDataProviderRepository _marketDataProviderRepository;

        public MarketDataProviderManager(IMarketDataProviderRepository marketDataProviderRepository)
        {
            _marketDataProviderRepository = marketDataProviderRepository;
        }

        public async Task<MarketDataProvider> CreateAsync(string name, string description)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existingMarketDataProvider = await _marketDataProviderRepository.FindByNameAsync(name);
            if (existingMarketDataProvider != null)
            {
                throw new MarketDataProviderAlreadyExistsException(name);
            }

            return new MarketDataProvider(Guid.NewGuid(), name, description);
        }

        public async Task UpdateNameAsync(MarketDataProvider marketDataProvider, string newName)
        {
            Check.NotNull(marketDataProvider, nameof(marketDataProvider));
            Check.NotNullOrWhiteSpace(newName, nameof(newName), MarketDataProviderConsts.MaxNameLength, MarketDataProviderConsts.MinNameLength);

            var existingMarketDataProvider = await _marketDataProviderRepository.FindByNameAsync(newName);
            if (existingMarketDataProvider != null)
            {
                throw new MarketDataProviderAlreadyExistsException(newName);
            }

            marketDataProvider.Name = newName;
        }
    }
}
