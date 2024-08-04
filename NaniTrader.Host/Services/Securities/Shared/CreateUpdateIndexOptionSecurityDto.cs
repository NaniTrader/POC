using NaniTrader.Entities.Securities.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NaniTrader.Services.Securities.Shared
{
    public class CreateUpdateIndexOptionSecurityDto
    {
        [Required]
        [StringLength(SecurityConsts.MaxNameLength)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(SecurityConsts.MaxDescriptionLength)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public Guid ParentId { get; set; }

        [Required]
        public Guid UnderlyingId { get; set; }
    }
}
