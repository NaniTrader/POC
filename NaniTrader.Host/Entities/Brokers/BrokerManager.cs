using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;
using Volo.Abp;

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
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existingBroker = await _brokerRepository.FindByNameAsync(name);
            if (existingBroker != null)
            {
                throw new BrokerAlreadyExistsException(name);
            }

            return new Broker(Guid.NewGuid(), name, description);
        }
    }
}
