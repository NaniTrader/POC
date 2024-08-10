using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;
using Volo.Abp;
using NaniTrader.Entities.Brokers;

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
            Check.NotNullOrWhiteSpace(name, nameof(name), SecurityConsts.MaxNameLength, SecurityConsts.MinNameLength);
            Check.NotNullOrWhiteSpace(description, nameof(description), SecurityConsts.MaxDescriptionLength, SecurityConsts.MinDescriptionLength);

            var existingEquitySecurity = await _equitySecurityRepository.FindByParentIdAndNameAsync(parentId, name);
            if (existingEquitySecurity != null)
            {
                throw new SecurityAlreadyExistsException(parentId, name);
            }

            return new EquitySecurity(Guid.NewGuid(), parentId, name, description);
        }

        public async Task UpdateEquitySecurityNameAsync(EquitySecurity equitySecurity, string newName)
        {
            Check.NotNull(equitySecurity, nameof(equitySecurity));
            Check.NotNullOrWhiteSpace(newName, nameof(newName), SecurityConsts.MaxNameLength, SecurityConsts.MinNameLength);

            var existingEquitySecurity = await _equitySecurityRepository.FindByParentIdAndNameAsync(equitySecurity.ParentId, newName);
            if (existingEquitySecurity != null)
            {
                throw new SecurityAlreadyExistsException(equitySecurity.ParentId, newName);
            }

            equitySecurity.Name = newName;
        }

        public async Task<EquityFutureSecurity> CreateEquityFutureSecurityAsync(Guid underlyingId, Guid parentId, string name, string description)
        {
            Check.NotDefaultOrNull<Guid>(underlyingId, nameof(underlyingId));
            Check.NotDefaultOrNull<Guid>(parentId, nameof(parentId));
            Check.NotNullOrWhiteSpace(name, nameof(name), SecurityConsts.MaxNameLength, SecurityConsts.MinNameLength);
            Check.NotNullOrWhiteSpace(description, nameof(description), SecurityConsts.MaxDescriptionLength, SecurityConsts.MinDescriptionLength);

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

        public async Task UpdateEquityFutureSecurityNameAsync(EquityFutureSecurity equityFutureSecurity, string newName)
        {
            Check.NotNull(equityFutureSecurity, nameof(equityFutureSecurity));
            Check.NotNullOrWhiteSpace(newName, nameof(newName), SecurityConsts.MaxNameLength, SecurityConsts.MinNameLength);

            var existingEquityFutureSecurity = await _equityFutureSecurityRepository.FindByParentIdAndNameAsync(equityFutureSecurity.ParentId, newName);
            if (existingEquityFutureSecurity != null)
            {
                throw new SecurityAlreadyExistsException(equityFutureSecurity.ParentId, newName);
            }

            equityFutureSecurity.Name = newName;
        }

        public async Task<EquityOptionSecurity> CreateEquityOptionSecurityAsync(Guid underlyingId, Guid parentId, string name, string description)
        {
            Check.NotDefaultOrNull<Guid>(underlyingId, nameof(underlyingId));
            Check.NotDefaultOrNull<Guid>(parentId, nameof(parentId));
            Check.NotNullOrWhiteSpace(name, nameof(name), SecurityConsts.MaxNameLength, SecurityConsts.MinNameLength);
            Check.NotNullOrWhiteSpace(description, nameof(description), SecurityConsts.MaxDescriptionLength, SecurityConsts.MinDescriptionLength);

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

        public async Task UpdateEquityOptionSecurityNameAsync(EquityOptionSecurity equityOptionSecurity, string newName)
        {
            Check.NotNull(equityOptionSecurity, nameof(equityOptionSecurity));
            Check.NotNullOrWhiteSpace(newName, nameof(newName), SecurityConsts.MaxNameLength, SecurityConsts.MinNameLength);

            var existingEquityOptionSecurity = await _equityOptionSecurityRepository.FindByParentIdAndNameAsync(equityOptionSecurity.ParentId, newName);
            if (existingEquityOptionSecurity != null)
            {
                throw new SecurityAlreadyExistsException(equityOptionSecurity.ParentId, newName);
            }

            equityOptionSecurity.Name = newName;
        }

        public async Task<IndexSecurity> CreateIndexSecurityAsync(Guid parentId, string name, string description)
        {
            Check.NotDefaultOrNull<Guid>(parentId, nameof(parentId));
            Check.NotNullOrWhiteSpace(name, nameof(name), SecurityConsts.MaxNameLength, SecurityConsts.MinNameLength);
            Check.NotNullOrWhiteSpace(description, nameof(description), SecurityConsts.MaxDescriptionLength, SecurityConsts.MinDescriptionLength);

            var existingIndexSecurity = await _indexSecurityRepository.FindByParentIdAndNameAsync(parentId, name);
            if (existingIndexSecurity != null)
            {
                throw new SecurityAlreadyExistsException(parentId, name);
            }

            return new IndexSecurity(Guid.NewGuid(), parentId, name, description);
        }

        public async Task UpdateIndexSecurityNameAsync(IndexSecurity indexSecurity, string newName)
        {
            Check.NotNull(indexSecurity, nameof(indexSecurity));
            Check.NotNullOrWhiteSpace(newName, nameof(newName), SecurityConsts.MaxNameLength, SecurityConsts.MinNameLength);

            var existingIndexSecurity = await _indexSecurityRepository.FindByParentIdAndNameAsync(indexSecurity.ParentId, newName);
            if (existingIndexSecurity != null)
            {
                throw new SecurityAlreadyExistsException(indexSecurity.ParentId, newName);
            }

            indexSecurity.Name = newName;
        }

        public async Task<IndexFutureSecurity> CreateIndexFutureSecurityAsync(Guid underlyingId, Guid parentId, string name, string description)
        {
            Check.NotDefaultOrNull<Guid>(underlyingId, nameof(underlyingId));
            Check.NotDefaultOrNull<Guid>(parentId, nameof(parentId));
            Check.NotNullOrWhiteSpace(name, nameof(name), SecurityConsts.MaxNameLength, SecurityConsts.MinNameLength);
            Check.NotNullOrWhiteSpace(description, nameof(description), SecurityConsts.MaxDescriptionLength, SecurityConsts.MinDescriptionLength);

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

        public async Task UpdateIndexFutureSecurityNameAsync(IndexFutureSecurity indexFutureSecurity, string newName)
        {
            Check.NotNull(indexFutureSecurity, nameof(indexFutureSecurity));
            Check.NotNullOrWhiteSpace(newName, nameof(newName), SecurityConsts.MaxNameLength, SecurityConsts.MinNameLength);

            var existingIndexFutureSecurity = await _indexFutureSecurityRepository.FindByParentIdAndNameAsync(indexFutureSecurity.ParentId, newName);
            if (existingIndexFutureSecurity != null)
            {
                throw new SecurityAlreadyExistsException(indexFutureSecurity.ParentId, newName);
            }

            indexFutureSecurity.Name = newName;
        }

        public async Task<IndexOptionSecurity> CreateIndexOptionSecurityAsync(Guid underlyingId, Guid parentId, string name, string description)
        {
            Check.NotDefaultOrNull<Guid>(underlyingId, nameof(underlyingId));
            Check.NotDefaultOrNull<Guid>(parentId, nameof(parentId));
            Check.NotNullOrWhiteSpace(name, nameof(name), SecurityConsts.MaxNameLength, SecurityConsts.MinNameLength);
            Check.NotNullOrWhiteSpace(description, nameof(description), SecurityConsts.MaxDescriptionLength, SecurityConsts.MinDescriptionLength);

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

        public async Task UpdateIndexOptionSecurityNameAsync(IndexOptionSecurity indexOptionSecurity, string newName)
        {
            Check.NotNull(indexOptionSecurity, nameof(indexOptionSecurity));
            Check.NotNullOrWhiteSpace(newName, nameof(newName), SecurityConsts.MaxNameLength, SecurityConsts.MinNameLength);

            var existingIndexOptionSecurity = await _indexOptionSecurityRepository.FindByParentIdAndNameAsync(indexOptionSecurity.ParentId, newName);
            if (existingIndexOptionSecurity != null)
            {
                throw new SecurityAlreadyExistsException(indexOptionSecurity.ParentId, newName);
            }

            indexOptionSecurity.Name = newName;
        }
    }
}
