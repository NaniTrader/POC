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
        private readonly IEquitySecurityRepository _equitySecurityRepository;
        private readonly IEquityOptionSecurityRepository _equityOptionSecurityRepository;
        private readonly IEquityFutureSecurityRepository _equityFutureSecurityRepository;
        private readonly IIndexSecurityRepository _indexSecurityRepository;
        private readonly IIndexOptionSecurityRepository _indexOptionSecurityRepository;
        private readonly IIndexFutureSecurityRepository _indexFutureSecurityRepository;

        public SecurityManager(IEquitySecurityRepository equitySecurityRepository,
            IEquityOptionSecurityRepository equityOptionSecurityRepository,
            IEquityFutureSecurityRepository equityFutureSecurityRepository,
            IIndexSecurityRepository indexSecurityRepository,
            IIndexOptionSecurityRepository indexOptionSecurityRepository,
            IIndexFutureSecurityRepository indexFutureSecurityRepository)
        {
            _equitySecurityRepository = equitySecurityRepository;
            _equityOptionSecurityRepository = equityOptionSecurityRepository;
            _equityFutureSecurityRepository = equityFutureSecurityRepository;
            _indexSecurityRepository = indexSecurityRepository;
            _indexOptionSecurityRepository = indexOptionSecurityRepository;
            _indexFutureSecurityRepository = indexFutureSecurityRepository;
        }

        public async Task<EquitySecurity> CreateEquitySecurityAsync(Guid parentId, string name, string description)
        {
            Check.NotDefaultOrNull<Guid>(parentId, nameof(parentId));
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existingEquitySecurity = await _equitySecurityRepository.FindByParentIdAndNameAsync(parentId, name);
            if (existingEquitySecurity != null)
            {
                throw new SecurityAlreadyExistsException(parentId, name);
            }

            return new EquitySecurity(Guid.NewGuid(), parentId, name, description);
        }

        public async Task<EquityFutureSecurity> CreateEquityFutureSecurityAsync(Guid underlyingId, Guid parentId, string name, string description)
        {
            Check.NotDefaultOrNull<Guid>(underlyingId, nameof(underlyingId));
            Check.NotDefaultOrNull<Guid>(parentId, nameof(parentId));
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existingEquityFutureSecurity = await _equityFutureSecurityRepository.FindByParentIdAndNameAsync(parentId, name);
            if (existingEquityFutureSecurity != null)
            {
                throw new SecurityAlreadyExistsException(parentId, name);
            }

            var underlyingSecurity = await _equitySecurityRepository.GetAsync(underlyingId);
            if (underlyingSecurity == null)
            {
                throw new SecurityNotFoundException(underlyingId);
            }

            return new EquityFutureSecurity(Guid.NewGuid(), parentId, underlyingSecurity, name, description);
        }

        public async Task<EquityOptionSecurity> CreateEquityOptionSecurityAsync(Guid underlyingId, Guid parentId, string name, string description)
        {
            Check.NotDefaultOrNull<Guid>(underlyingId, nameof(underlyingId));
            Check.NotDefaultOrNull<Guid>(parentId, nameof(parentId));
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existingEquityOptionSecurity = await _equityOptionSecurityRepository.FindByParentIdAndNameAsync(parentId, name);
            if (existingEquityOptionSecurity != null)
            {
                throw new SecurityAlreadyExistsException(parentId, name);
            }

            var underlyingSecurity = await _equitySecurityRepository.GetAsync(underlyingId);
            if (underlyingSecurity == null)
            {
                throw new SecurityNotFoundException(underlyingId);
            }

            return new EquityOptionSecurity(Guid.NewGuid(), parentId, underlyingSecurity, name, description);
        }

        public async Task<IndexSecurity> CreateIndexSecurityAsync(Guid parentId, string name, string description)
        {
            Check.NotDefaultOrNull<Guid>(parentId, nameof(parentId));
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existingIndexSecurity = await _indexSecurityRepository.FindByParentIdAndNameAsync(parentId, name);
            if (existingIndexSecurity != null)
            {
                throw new SecurityAlreadyExistsException(parentId, name);
            }

            return new IndexSecurity(Guid.NewGuid(), parentId, name, description);
        }

        public async Task<IndexFutureSecurity> CreateIndexFutureSecurityAsync(Guid underlyingId, Guid parentId, string name, string description)
        {
            Check.NotDefaultOrNull<Guid>(underlyingId, nameof(underlyingId));
            Check.NotDefaultOrNull<Guid>(parentId, nameof(parentId));
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existingIndexFutureSecurity = await _indexFutureSecurityRepository.FindByParentIdAndNameAsync(parentId, name);
            if (existingIndexFutureSecurity != null)
            {
                throw new SecurityAlreadyExistsException(parentId, name);
            }

            var underlyingSecurity = await _indexSecurityRepository.GetAsync(underlyingId);
            if (underlyingSecurity == null)
            {
                throw new SecurityNotFoundException(underlyingId);
            }

            return new IndexFutureSecurity(Guid.NewGuid(), parentId, underlyingSecurity, name, description);
        }

        public async Task<IndexOptionSecurity> CreateIndexOptionSecurityAsync(Guid underlyingId, Guid parentId, string name, string description)
        {
            Check.NotDefaultOrNull<Guid>(underlyingId, nameof(underlyingId));
            Check.NotDefaultOrNull<Guid>(parentId, nameof(parentId));
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existingIndexOptionSecurity = await _indexFutureSecurityRepository.FindByParentIdAndNameAsync(parentId, name);
            if (existingIndexOptionSecurity != null)
            {
                throw new SecurityAlreadyExistsException(parentId, name);
            }

            var underlyingSecurity = await _indexSecurityRepository.GetAsync(underlyingId);
            if (underlyingSecurity == null)
            {
                throw new SecurityNotFoundException(underlyingId);
            }

            return new IndexOptionSecurity(Guid.NewGuid(), parentId, underlyingSecurity, name, description);
        }
    }
}
