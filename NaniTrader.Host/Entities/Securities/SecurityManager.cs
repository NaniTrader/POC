using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;
using Volo.Abp;

namespace NaniTrader.Entities.Securities
{
    public class SecurityManager : DomainService
    {
        private readonly ISecurityRepository _securityRepository;

        public SecurityManager(ISecurityRepository securityRepository)
        {
            _securityRepository = securityRepository;
        }

        public async Task<EquitySecurity> CreateEquitySecurityAsync(Guid parentId, string name, string description)
        {
            Check.NotDefaultOrNull<Guid>(parentId, nameof(parentId));
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existingSecurity = await _securityRepository.FindByParentIdAndNameAsync(parentId, name);
            if (existingSecurity != null)
            {
                throw new SecurityAlreadyExistsException(parentId, name);
            }

            return new EquitySecurity(Guid.NewGuid(), name, description);
        }
    }
}
