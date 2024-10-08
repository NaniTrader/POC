﻿using Blazorise;
using Microsoft.AspNetCore.Authorization;
using NaniTrader.Entities.Brokers;
using NaniTrader.Entities.Securities;
using NaniTrader.Services.Brokers;
using NaniTrader.Services.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace NaniTrader.Services.Securities
{
    [Authorize(NaniTraderPermissions.Securities.Default)]
    public class SecurityAppService : NaniTraderAppService, ISecurityAppService
    {
        // TODO Assess logic before updating the parentId or underlyingId for a given security.
        // Underlying future and option should have same parent type at any given time
        private readonly IEquitySecurityRepository _equitySecurityRepository;
        private readonly IEquityFutureSecurityRepository _equityFutureSecurityRepository;
        private readonly IEquityOptionSecurityRepository _equityOptionSecurityRepository;
        private readonly IIndexSecurityRepository _indexSecurityRepository;
        private readonly IIndexFutureSecurityRepository _indexFutureSecurityRepository;
        private readonly IIndexOptionSecurityRepository _indexOptionSecurityRepository;
        private readonly SecurityManager _securityManager;

        public SecurityAppService(
            IEquitySecurityRepository equitySecurityRepository,
            IEquityFutureSecurityRepository equityFutureSecurityRepository,
            IEquityOptionSecurityRepository equityOptionSecurityRepository,
            IIndexSecurityRepository indexSecurityRepository,
            IIndexFutureSecurityRepository indexFutureSecurityRepository,
            IIndexOptionSecurityRepository indexOptionSecurityRepository,
            SecurityManager securityManager)
        {
            _equitySecurityRepository = equitySecurityRepository;
            _equityFutureSecurityRepository = equityFutureSecurityRepository;
            _equityOptionSecurityRepository = equityOptionSecurityRepository;
            _indexSecurityRepository = indexSecurityRepository;
            _indexFutureSecurityRepository = indexFutureSecurityRepository;
            _indexOptionSecurityRepository = indexOptionSecurityRepository;
            _securityManager = securityManager;
        }

        public async Task<EquitySecurityDto> GetEquitySecurityAsync(Guid id)
        {
            var equitySecurity = await _equitySecurityRepository.GetAsync(id);
            return ObjectMapper.Map<EquitySecurity, EquitySecurityDto>(equitySecurity);
        }

        public async Task<PagedResultDto<EquitySecurityInListDto>> GetEquitySecurityPagedListWithNameFilterAsync(EquitySecurityListFilterDto input)
        {
            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(EquitySecurity.Name);
            }

            var equitySecurities = await _equitySecurityRepository.GetPagedListWithNameFilterAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting,
                input.Name
            );

            var totalCount = input.Name == null
                ? await _equitySecurityRepository.CountAsync()
                : await _equitySecurityRepository.CountAsync(
                    broker => broker.Name.Contains(input.Name));

            return new PagedResultDto<EquitySecurityInListDto>(
                totalCount,
                ObjectMapper.Map<List<EquitySecurity>, List<EquitySecurityInListDto>>(equitySecurities)
            );
        }

        [Authorize(NaniTraderPermissions.Securities.Create)]
        public async Task<EquitySecurityDto> CreateEquitySecurityAsync(CreateUpdateEquitySecurityDto input)
        {
            var equitySecurity = await _securityManager.CreateEquitySecurityAsync(
                input.ParentId,
                input.Name,
                input.Description
            );

            await _equitySecurityRepository.InsertAsync(equitySecurity);

            return ObjectMapper.Map<EquitySecurity, EquitySecurityDto>(equitySecurity);
        }

        [Authorize(NaniTraderPermissions.Securities.Edit)]
        public async Task UpdateEquitySecurityAsync(Guid id, CreateUpdateEquitySecurityDto input)
        {
            var equitySecurity = await _equitySecurityRepository.GetAsync(id);

            if (equitySecurity.Name != input.Name)
            {
                await _securityManager.UpdateEquitySecurityNameAsync(equitySecurity, input.Name);
            }

            if (equitySecurity.ParentId != input.ParentId)
            {
                await _securityManager.UpdateEquitySecurityParentIdAsync(equitySecurity, input.ParentId);
            }

            await _equitySecurityRepository.UpdateAsync(equitySecurity);
        }

        [Authorize(NaniTraderPermissions.Securities.Delete)]
        public async Task DeleteEquitySecurityAsync(Guid id)
        {
            await _equitySecurityRepository.DeleteAsync(id);
        }

        public async Task<EquityFutureSecurityDto> GetEquityFutureSecurityAsync(Guid id)
        {
            var equityFutureSecurity = await _equityFutureSecurityRepository.GetAsync(id);
            await _equityFutureSecurityRepository.EnsurePropertyLoadedAsync(equityFutureSecurity, x => x.Underlying);
            return ObjectMapper.Map<EquityFutureSecurity, EquityFutureSecurityDto>(equityFutureSecurity);
        }

        public async Task<PagedResultDto<EquityFutureSecurityInListDto>> GetEquityFutureSecurityPagedListWithNameFilterAsync(EquityFutureSecurityListFilterDto input)
        {
            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(EquityFutureSecurity.Name);
            }

            var equityFutureSecurities = await _equityFutureSecurityRepository.GetPagedListWithNameFilterAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting,
                input.Name
            );

            var totalCount = input.Name == null
                ? await _equityFutureSecurityRepository.CountAsync()
                : await _equityFutureSecurityRepository.CountAsync(
                    broker => broker.Name.Contains(input.Name));

            return new PagedResultDto<EquityFutureSecurityInListDto>(
                totalCount,
                ObjectMapper.Map<List<EquityFutureSecurity>, List<EquityFutureSecurityInListDto>>(equityFutureSecurities)
            );
        }

        [Authorize(NaniTraderPermissions.Securities.Create)]
        public async Task<EquityFutureSecurityDto> CreateEquityFutureSecurityAsync(CreateUpdateEquityFutureSecurityDto input)
        {
            var equityFutureSecurity = await _securityManager.CreateEquityFutureSecurityAsync(
                input.UnderlyingId,
                input.ParentId,
                input.Name,
                input.Description
            );

            await _equityFutureSecurityRepository.InsertAsync(equityFutureSecurity);

            return ObjectMapper.Map<EquityFutureSecurity, EquityFutureSecurityDto>(equityFutureSecurity);
        }

        [Authorize(NaniTraderPermissions.Securities.Edit)]
        public async Task UpdateEquityFutureSecurityAsync(Guid id, CreateUpdateEquityFutureSecurityDto input)
        {
            var equityFutureSecurity = await _equityFutureSecurityRepository.GetAsync(id);
            await _equityFutureSecurityRepository.EnsurePropertyLoadedAsync(equityFutureSecurity, x => x.Underlying);

            if (equityFutureSecurity.Name != input.Name)
            {
                await _securityManager.UpdateEquityFutureSecurityNameAsync(equityFutureSecurity, input.Name);
            }

            if (equityFutureSecurity.ParentId != input.ParentId)
            {
                await _securityManager.UpdateEquityFutureSecurityParentIdAsync(equityFutureSecurity, input.ParentId);
            }

            if (equityFutureSecurity.Underlying.Id != input.UnderlyingId)
            {
                await _securityManager.UpdateEquityFutureSecurityUnderlyingIdAsync(equityFutureSecurity, input.UnderlyingId);
            }

            await _equityFutureSecurityRepository.UpdateAsync(equityFutureSecurity);
        }

        [Authorize(NaniTraderPermissions.Securities.Delete)]
        public async Task DeleteEquityFutureSecurityAsync(Guid id)
        {
            await _equityFutureSecurityRepository.DeleteAsync(id);
        }

        public async Task<EquityOptionSecurityDto> GetEquityOptionSecurityAsync(Guid id)
        {
            var equityOptionSecurity = await _equityOptionSecurityRepository.GetAsync(id);
            await _equityOptionSecurityRepository.EnsurePropertyLoadedAsync(equityOptionSecurity, x => x.Underlying);
            return ObjectMapper.Map<EquityOptionSecurity, EquityOptionSecurityDto>(equityOptionSecurity);
        }

        public async Task<PagedResultDto<EquityOptionSecurityInListDto>> GetEquityOptionSecurityPagedListWithNameFilterAsync(EquityOptionSecurityListFilterDto input)
        {
            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(EquityOptionSecurity.Name);
            }

            var equityOptionSecurities = await _equityOptionSecurityRepository.GetPagedListWithNameFilterAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting,
                input.Name
            );

            var totalCount = input.Name == null
                ? await _equityOptionSecurityRepository.CountAsync()
                : await _equityOptionSecurityRepository.CountAsync(
                    broker => broker.Name.Contains(input.Name));

            return new PagedResultDto<EquityOptionSecurityInListDto>(
                totalCount,
                ObjectMapper.Map<List<EquityOptionSecurity>, List<EquityOptionSecurityInListDto>>(equityOptionSecurities)
            );
        }

        [Authorize(NaniTraderPermissions.Securities.Create)]
        public async Task<EquityOptionSecurityDto> CreateEquityOptionSecurityAsync(CreateUpdateEquityOptionSecurityDto input)
        {
            var equityOptionSecurity = await _securityManager.CreateEquityOptionSecurityAsync(
                input.UnderlyingId,
                input.ParentId,
                input.Name,
                input.Description
            );

            await _equityOptionSecurityRepository.InsertAsync(equityOptionSecurity);

            return ObjectMapper.Map<EquityOptionSecurity, EquityOptionSecurityDto>(equityOptionSecurity);
        }

        [Authorize(NaniTraderPermissions.Securities.Edit)]
        public async Task UpdateEquityOptionSecurityAsync(Guid id, CreateUpdateEquityOptionSecurityDto input)
        {
            var equityOptionSecurity = await _equityOptionSecurityRepository.GetAsync(id);
            await _equityOptionSecurityRepository.EnsurePropertyLoadedAsync(equityOptionSecurity, x => x.Underlying);

            if (equityOptionSecurity.Name != input.Name)
            {
                await _securityManager.UpdateEquityOptionSecurityNameAsync(equityOptionSecurity, input.Name);
            }

            if (equityOptionSecurity.ParentId != input.ParentId)
            {
                await _securityManager.UpdateEquityOptionSecurityParentIdAsync(equityOptionSecurity, input.ParentId);
            }

            if (equityOptionSecurity.Underlying.Id != input.UnderlyingId)
            {
                await _securityManager.UpdateEquityOptionSecurityUnderlyingIdAsync(equityOptionSecurity, input.UnderlyingId);
            }

            await _equityOptionSecurityRepository.UpdateAsync(equityOptionSecurity);
        }

        [Authorize(NaniTraderPermissions.Securities.Delete)]
        public async Task DeleteEquityOptionSecurityAsync(Guid id)
        {
            await _equityOptionSecurityRepository.DeleteAsync(id);
        }

        public async Task<IndexSecurityDto> GetIndexSecurityAsync(Guid id)
        {
            var indexSecurity = await _indexSecurityRepository.GetAsync(id);
            return ObjectMapper.Map<IndexSecurity, IndexSecurityDto>(indexSecurity);
        }

        public async Task<PagedResultDto<IndexSecurityInListDto>> GetIndexSecurityPagedListWithNameFilterAsync(IndexSecurityListFilterDto input)
        {
            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(IndexSecurity.Name);
            }

            var indexSecurities = await _indexSecurityRepository.GetPagedListWithNameFilterAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting,
                input.Name
            );

            var totalCount = input.Name == null
                ? await _indexSecurityRepository.CountAsync()
                : await _indexSecurityRepository.CountAsync(
                    broker => broker.Name.Contains(input.Name));

            return new PagedResultDto<IndexSecurityInListDto>(
                totalCount,
                ObjectMapper.Map<List<IndexSecurity>, List<IndexSecurityInListDto>>(indexSecurities)
            );
        }

        [Authorize(NaniTraderPermissions.Securities.Create)]
        public async Task<IndexSecurityDto> CreateIndexSecurityAsync(CreateUpdateIndexSecurityDto input)
        {
            var indexSecurity = await _securityManager.CreateIndexSecurityAsync(
                input.ParentId,
                input.Name,
                input.Description
            );

            await _indexSecurityRepository.InsertAsync(indexSecurity);

            return ObjectMapper.Map<IndexSecurity, IndexSecurityDto>(indexSecurity);
        }

        [Authorize(NaniTraderPermissions.Securities.Edit)]
        public async Task UpdateIndexSecurityAsync(Guid id, CreateUpdateIndexSecurityDto input)
        {
            var indexSecurity = await _indexSecurityRepository.GetAsync(id);

            if (indexSecurity.Name != input.Name)
            {
                await _securityManager.UpdateIndexSecurityNameAsync(indexSecurity, input.Name);
            }

            if (indexSecurity.ParentId != input.ParentId)
            {
                await _securityManager.UpdateIndexSecurityParentIdAsync(indexSecurity, input.ParentId);
            }

            await _indexSecurityRepository.UpdateAsync(indexSecurity);
        }

        [Authorize(NaniTraderPermissions.Securities.Delete)]
        public async Task DeleteIndexSecurityAsync(Guid id)
        {
            await _indexSecurityRepository.DeleteAsync(id);
        }

        public async Task<IndexFutureSecurityDto> GetIndexFutureSecurityAsync(Guid id)
        {
            var indexFutureSecurity = await _indexFutureSecurityRepository.GetAsync(id);
            await _indexFutureSecurityRepository.EnsurePropertyLoadedAsync(indexFutureSecurity, x => x.Underlying);
            return ObjectMapper.Map<IndexFutureSecurity, IndexFutureSecurityDto>(indexFutureSecurity);
        }

        public async Task<PagedResultDto<IndexFutureSecurityInListDto>> GetIndexFutureSecurityPagedListWithNameFilterAsync(IndexFutureSecurityListFilterDto input)
        {
            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(IndexFutureSecurity.Name);
            }

            var indexFutureSecurities = await _indexFutureSecurityRepository.GetPagedListWithNameFilterAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting,
                input.Name
            );

            var totalCount = input.Name == null
                ? await _indexFutureSecurityRepository.CountAsync()
                : await _indexFutureSecurityRepository.CountAsync(
                    broker => broker.Name.Contains(input.Name));

            return new PagedResultDto<IndexFutureSecurityInListDto>(
                totalCount,
                ObjectMapper.Map<List<IndexFutureSecurity>, List<IndexFutureSecurityInListDto>>(indexFutureSecurities)
            );
        }

        [Authorize(NaniTraderPermissions.Securities.Create)]
        public async Task<IndexFutureSecurityDto> CreateIndexFutureSecurityAsync(CreateUpdateIndexFutureSecurityDto input)
        {
            var indexFutureSecurity = await _securityManager.CreateIndexFutureSecurityAsync(
                input.UnderlyingId,
                input.ParentId,
                input.Name,
                input.Description
            );

            await _indexFutureSecurityRepository.InsertAsync(indexFutureSecurity);

            return ObjectMapper.Map<IndexFutureSecurity, IndexFutureSecurityDto>(indexFutureSecurity);
        }

        [Authorize(NaniTraderPermissions.Securities.Edit)]
        public async Task UpdateIndexFutureSecurityAsync(Guid id, CreateUpdateIndexFutureSecurityDto input)
        {
            var indexFutureSecurity = await _indexFutureSecurityRepository.GetAsync(id);
            await _indexFutureSecurityRepository.EnsurePropertyLoadedAsync(indexFutureSecurity, x => x.Underlying);

            if (indexFutureSecurity.Name != input.Name)
            {
                await _securityManager.UpdateIndexFutureSecurityNameAsync(indexFutureSecurity, input.Name);
            }

            if (indexFutureSecurity.ParentId != input.ParentId)
            {
                await _securityManager.UpdateIndexFutureSecurityParentIdAsync(indexFutureSecurity, input.ParentId);
            }

            if (indexFutureSecurity.Underlying.Id != input.UnderlyingId)
            {
                await _securityManager.UpdateIndexFutureSecurityUnderlyingIdAsync(indexFutureSecurity, input.UnderlyingId);
            }

            await _indexFutureSecurityRepository.UpdateAsync(indexFutureSecurity);
        }

        [Authorize(NaniTraderPermissions.Securities.Delete)]
        public async Task DeleteIndexFutureSecurityAsync(Guid id)
        {
            await _indexFutureSecurityRepository.DeleteAsync(id);
        }

        public async Task<IndexOptionSecurityDto> GetIndexOptionSecurityAsync(Guid id)
        {
            var indexOptionSecurity = await _indexOptionSecurityRepository.GetAsync(id);
            await _indexOptionSecurityRepository.EnsurePropertyLoadedAsync(indexOptionSecurity, x => x.Underlying);
            return ObjectMapper.Map<IndexOptionSecurity, IndexOptionSecurityDto>(indexOptionSecurity);
        }

        public async Task<PagedResultDto<IndexOptionSecurityInListDto>> GetIndexOptionSecurityPagedListWithNameFilterAsync(IndexOptionSecurityListFilterDto input)
        {
            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(IndexOptionSecurity.Name);
            }

            var indexOptionSecurities = await _indexOptionSecurityRepository.GetPagedListWithNameFilterAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting,
                input.Name
            );

            var totalCount = input.Name == null
                ? await _indexOptionSecurityRepository.CountAsync()
                : await _indexOptionSecurityRepository.CountAsync(
                    broker => broker.Name.Contains(input.Name));

            return new PagedResultDto<IndexOptionSecurityInListDto>(
                totalCount,
                ObjectMapper.Map<List<IndexOptionSecurity>, List<IndexOptionSecurityInListDto>>(indexOptionSecurities)
            );
        }

        [Authorize(NaniTraderPermissions.Securities.Create)]
        public async Task<IndexOptionSecurityDto> CreateIndexOptionSecurityAsync(CreateUpdateIndexOptionSecurityDto input)
        {
            var indexOptionSecurity = await _securityManager.CreateIndexOptionSecurityAsync(
                input.UnderlyingId,
                input.ParentId,
                input.Name,
                input.Description
            );

            await _indexOptionSecurityRepository.InsertAsync(indexOptionSecurity);

            return ObjectMapper.Map<IndexOptionSecurity, IndexOptionSecurityDto>(indexOptionSecurity);
        }

        [Authorize(NaniTraderPermissions.Securities.Edit)]
        public async Task UpdateIndexOptionSecurityAsync(Guid id, CreateUpdateIndexOptionSecurityDto input)
        {
            var indexOptionSecurity = await _indexOptionSecurityRepository.GetAsync(id);
            await _indexOptionSecurityRepository.EnsurePropertyLoadedAsync(indexOptionSecurity, x => x.Underlying);

            if (indexOptionSecurity.Name != input.Name)
            {
                await _securityManager.UpdateIndexOptionSecurityNameAsync(indexOptionSecurity, input.Name);
            }

            if (indexOptionSecurity.ParentId != input.ParentId)
            {
                await _securityManager.UpdateIndexOptionSecurityParentIdAsync(indexOptionSecurity, input.ParentId);
            }

            if (indexOptionSecurity.Underlying.Id != input.UnderlyingId)
            {
                await _securityManager.UpdateIndexOptionSecurityUnderlyingIdAsync(indexOptionSecurity, input.UnderlyingId);
            }

            await _indexOptionSecurityRepository.UpdateAsync(indexOptionSecurity);
        }

        [Authorize(NaniTraderPermissions.Securities.Delete)]
        public async Task DeleteIndexOptionSecurityAsync(Guid id)
        {
            await _indexOptionSecurityRepository.DeleteAsync(id);
        }

    }
}
