﻿using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace NaniTrader.Services.Securities
{
    public class IndexFutureSecurityDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Guid ParentId { get; set; }

        public Guid UnderlyingId { get; set; }
    }
}
