using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace NaniTrader.Services.Securities
{
    public class IndexOptionSecurityListFilterDto : PagedAndSortedResultRequestDto
    {
        public string? Name { get; set; }
    }
}
