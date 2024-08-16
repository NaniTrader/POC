using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaniTrader.Entities.Shared;
using Volo.Abp;

namespace NaniTrader.Entities.Securities
{
    public class SecurityNotFoundException : BusinessException
    {
        public SecurityNotFoundException(Guid id)
            : base(NaniTraderDomainErrorCodes.SecurityNotFound)
        {
            WithData("Id", id);
        }
    }
}
