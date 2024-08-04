using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;
using Volo.Abp;
using System.Diagnostics.CodeAnalysis;

namespace NaniTrader.Entities.Brokers
{
    public class BrokerManager : DomainService
    {
        private readonly IBrokerRepository _brokerRepository;

        public BrokerManager(IBrokerRepository brokerRepository)
        {
            _brokerRepository = brokerRepository;
        }

        public async Task<Broker> CreateAsync(string name, string description)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name), BrokerConsts.MaxNameLength, BrokerConsts.MinNameLength);
            Check.NotNullOrWhiteSpace(description, nameof(description), BrokerConsts.MaxDescriptionLength, BrokerConsts.MinDescriptionLength);

            var existingBroker = await _brokerRepository.FindByNameAsync(name);
            if (existingBroker != null)
            {
                throw new BrokerAlreadyExistsException(name);
            }

            return new Broker(Guid.NewGuid(), name, description);
        }

        public async Task UpdateNameAsync(Broker broker, string newName)
        {
            Check.NotNull(broker, nameof(broker));
            Check.NotNullOrWhiteSpace(newName, nameof(newName), BrokerConsts.MaxNameLength, BrokerConsts.MinNameLength);

            var existingBroker = await _brokerRepository.FindByNameAsync(newName);
            if (existingBroker != null)
            {
                throw new BrokerAlreadyExistsException(newName);
            }

            broker.Name = newName;
        }
    }
}
