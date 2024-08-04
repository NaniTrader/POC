using NaniTrader.Entities.Brokers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NaniTrader.Services.Brokers
{
    public class CreateUpdateBrokerDto
    {
        [Required]
        [StringLength(BrokerConsts.MaxNameLength)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(BrokerConsts.MaxDescriptionLength)]
        public string Description { get; set; } = string.Empty;
    }
}
