using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaniTrader.Entities.Shared;
using Volo.Abp;

namespace NaniTrader.Entities.MarketData
{
    public class MarketDataProviderAlreadyExistsException : BusinessException
    {
        public MarketDataProviderAlreadyExistsException(string name)
            : base(NaniTraderDomainErrorCodes.MarketDataProviderAlreadyExists)
        {
            WithData("name", name);
        }
    }
}
