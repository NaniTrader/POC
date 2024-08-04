using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace NaniTrader.Services.Securities
{
    public class IndexFutureSecurityInListDto : AuditedEntityDto<int>
    {
        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}
