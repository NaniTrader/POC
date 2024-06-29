using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaniTrader.Entities.Shared;
using Volo.Abp;

namespace NaniTrader.Entities.Exchanges
{
    public class ExchangeAlreadyExistsException : BusinessException
    {
        public ExchangeAlreadyExistsException(string name)
            : base(NaniTraderDomainErrorCodes.ExchangeAlreadyExists)
        {
            WithData("name", name);
        }
    }
}
