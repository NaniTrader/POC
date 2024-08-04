using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace NaniTrader.Services.Brokers
{
    public class BrokerListFilterDto : PagedAndSortedResultRequestDto
    {
        public string? Name { get; set; }
    }
}
