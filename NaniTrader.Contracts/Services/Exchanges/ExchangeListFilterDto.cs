﻿using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace NaniTrader.Services.Exchanges
{
    public class ExchangeListFilterDto : PagedAndSortedResultRequestDto
    {
        public string? Name { get; set; }
    }
}
