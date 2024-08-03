using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace NaniTrader.Services.Brokers.Shared
{
    public class BrokerInListDto : AuditedEntityDto<int>
    {
        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}
