using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaniTrader.Entities.Shared;
using Volo.Abp;

namespace NaniTrader.Entities.Brokers
{
    public class BrokerAlreadyExistsException : BusinessException
    {
        public BrokerAlreadyExistsException(string name)
            : base(NaniTraderDomainErrorCodes.BrokerAlreadyExists)
        {
            WithData("name", name);
        }
    }
}
