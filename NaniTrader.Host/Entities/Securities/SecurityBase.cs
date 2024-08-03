using Microsoft.Extensions.Logging;
using NaniTrader.Entities.Securities.Shared;
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

        public string Name { get; private set; }
        public string Description { get; private set; }
        public Guid ParentId { get; private set; }

        internal SecurityBase(Guid id, Guid parentId, string name, string description) : base(id)
        {
            SetName(name);
            SetDescription(description);
            SetParentId(parentId);
        }

        [MemberNotNull(nameof(Name))]
        public SecurityBase SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), SecurityConsts.MaxNameLength, SecurityConsts.MinNameLength);
            return this;
        }

        [MemberNotNull(nameof(Description))]
        public SecurityBase SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), SecurityConsts.MaxDescriptionLength, SecurityConsts.MinDescriptionLength);
            return this;
        }

        [MemberNotNull(nameof(ParentId))]
        public SecurityBase SetParentId(Guid parentId)
        {
            ParentId = Check.NotDefaultOrNull<Guid>(parentId, nameof(parentId));
            return this;
        }
    }
}
