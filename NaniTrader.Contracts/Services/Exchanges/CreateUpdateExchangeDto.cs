using NaniTrader.Entities.Exchanges;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NaniTrader.Services.Exchanges
{
    public class CreateUpdateExchangeDto
    {
        [Required]
        [StringLength(ExchangeConsts.MaxNameLength)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(ExchangeConsts.MaxDescriptionLength)]
        public string Description { get; set; } = string.Empty;
    }
}
