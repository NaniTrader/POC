using Microsoft.AspNetCore.Authorization;
using NaniTrader.Entities.Brokers;
using NaniTrader.Entities.Exchanges;
using NaniTrader.Services.Brokers;
using NaniTrader.Services.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace NaniTrader.Services.Exchanges
{
    [Authorize(NaniTraderPermissions.Exchanges.Default)]
    public class ExchangeAppService : NaniTraderAppService, IExchangeAppService
    {
        private readonly IExchangeRepository _exchangeRepository;
        private readonly ExchangeManager _exchangeManager;

        public ExchangeAppService(
            IExchangeRepository exchangeRepository,
            ExchangeManager exchangeManager)
        {
            _exchangeRepository = exchangeRepository;
            _exchangeManager = exchangeManager;
        }

        public async Task<ExchangeDto> GetAsync(Guid id)
        {
            var exchange = await _exchangeRepository.GetAsync(id);
            return ObjectMapper.Map<Exchange, ExchangeDto>(exchange);
        }

        public async Task<PagedResultDto<ExchangeInListDto>> GetPagedListWithNameFilterAsync(ExchangeListFilterDto input)
        {
            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(Exchange.Name);
            }

            var exchanges = await _exchangeRepository.GetPagedListWithNameFilterAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting,
                input.Name
            );

            var totalCount = input.Name == null
                ? await _exchangeRepository.CountAsync()
                : await _exchangeRepository.CountAsync(
                    exchange => exchange.Name.Contains(input.Name));

            return new PagedResultDto<ExchangeInListDto>(
                totalCount,
                ObjectMapper.Map<List<Exchange>, List<ExchangeInListDto>>(exchanges)
            );
        }

        [Authorize(NaniTraderPermissions.Exchanges.Create)]
        public async Task<ExchangeDto> CreateAsync(CreateUpdateExchangeDto input)
        {
            var exchange = await _exchangeManager.CreateAsync(
                input.Name,
                input.Description
            );

            await _exchangeRepository.InsertAsync(exchange);

            return ObjectMapper.Map<Exchange, ExchangeDto>(exchange);
        }

        [Authorize(NaniTraderPermissions.Exchanges.Edit)]
        public async Task UpdateAsync(Guid id, CreateUpdateExchangeDto input)
        {
            var broker = await _exchangeRepository.GetAsync(id);

            if (broker.Name != input.Name)
            {
                await _exchangeManager.UpdateNameAsync(broker, input.Name);
            }

            await _exchangeRepository.UpdateAsync(broker);
        }

        [Authorize(NaniTraderPermissions.Exchanges.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _exchangeRepository.DeleteAsync(id);
        }
    }
}
