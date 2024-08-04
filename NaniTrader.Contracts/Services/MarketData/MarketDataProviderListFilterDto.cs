using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace NaniTrader.Services.MarketData
{
    public class MarketDataProviderListFilterDto : PagedAndSortedResultRequestDto
    {
        public string? Name { get; set; }
    }
}
