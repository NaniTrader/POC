using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace NaniTrader.Services.Securities
{
    public class IndexSecurityListFilterDto : PagedAndSortedResultRequestDto
    {
        public string? Name { get; set; }
    }
}
