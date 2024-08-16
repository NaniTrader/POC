using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaniTrader.Entities.Shared;
using Volo.Abp;

namespace NaniTrader.Entities.Securities
{
    // TODO do not respond user error with id values
    public class SecurityAlreadyExistsException : BusinessException
    {
        public SecurityAlreadyExistsException(Guid parentId, string name)
            : base(NaniTraderDomainErrorCodes.SecurityAlreadyExists)
        {
            WithData("parentId", parentId);
            WithData("name", name);
        }
    }
}
