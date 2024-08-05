using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NaniTrader.Entities.Securities
{
    public class SecurityBase : FullAuditedAggregateRoot<Guid>
    {
        // here for ef core
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
        protected SecurityBase() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor.

        public string Name { get; internal set; }
        public string Description { get; internal set; }
        public Guid ParentId { get; internal set; }

        internal SecurityBase(Guid id, Guid parentId, string name, string description) : base(id)
        {
            Name = name;
            Description = description;
            ParentId = parentId;
        }
    }
}
