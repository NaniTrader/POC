﻿using NaniTrader.Entities.Exchanges;
using NaniTrader.Services.Exchanges.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace NaniTrader.Services.Exchanges
{
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

        public async Task<ExchangeDto> GetAsync(Ulid id)
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

        public async Task<ExchangeDto> CreateAsync(CreateUpdateExchangeDto input)
        {
            var exchange = await _exchangeManager.CreateAsync(
                input.Name,
                input.Description
            );

            await _exchangeRepository.InsertAsync(exchange);

            return ObjectMapper.Map<Exchange, ExchangeDto>(exchange);
        }

        public async Task DeleteAsync(Ulid id)
        {
            await _exchangeRepository.DeleteAsync(id);
        }
    }
}
