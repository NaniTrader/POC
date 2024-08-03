using NaniTrader.Entities.Securities;
using NaniTrader.Services.Securities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace NaniTrader.Services.Brokers
{
    public class SecurityAppService : NaniTraderAppService, ISecurityAppService
    {
        private readonly IEquitySecurityRepository _equitySecurityRepository;
        private readonly SecurityManager _securityManager;

        public SecurityAppService(
            IEquitySecurityRepository equitySecurityRepository,
            SecurityManager securityManager)
        {
            _equitySecurityRepository = equitySecurityRepository;
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

        public async Task DeleteEquitySecurityAsync(Guid id)
        {
            await _equitySecurityRepository.DeleteAsync(id);
        }
    }
}
