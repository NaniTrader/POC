﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;
using Volo.Abp;
using NaniTrader.Services.Misc.Shared;

namespace NaniTrader.Entities.Exchanges
{
    public class ExchangeManager : DomainService
    {
        private readonly IExchangeRepository _exchangeRepository;
        private readonly IUlidGeneratorService _ulidGeneratorService;

        public ExchangeManager(IExchangeRepository exchangeRepository, IUlidGeneratorService ulidGeneratorService)
        {
            _exchangeRepository = exchangeRepository;
            _ulidGeneratorService = ulidGeneratorService;
        }

        public async Task<Exchange> CreateAsync(string name, string description)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existingExchange = await _exchangeRepository.FindByNameAsync(name);
            if (existingExchange != null)
            {
                throw new ExchangeAlreadyExistsException(name);
            }

            return new Exchange(_ulidGeneratorService.Create(), name, description);
        }
    }
}
