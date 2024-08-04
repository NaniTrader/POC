using NaniTrader.Entities.MarketData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NaniTrader.Services.MarketData
{
    public class CreateUpdateMarketDataProviderDto
    {
        [Required]
        [StringLength(MarketDataProviderConsts.MaxNameLength)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(MarketDataProviderConsts.MaxDescriptionLength)]
        public string Description { get; set; } = string.Empty;
    }
}
