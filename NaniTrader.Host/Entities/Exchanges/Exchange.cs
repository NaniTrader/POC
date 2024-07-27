using Microsoft.Extensions.Logging;
using NaniTrader.Entities.Exchanges.Shared;
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

namespace NaniTrader.Entities.Exchanges
{
    public class Exchange : FullAuditedAggregateRoot<Guid>
    {
        // here for ef core
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
        private Exchange() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor.

        public string Name { get; private set; }
        public string Description { get; private set; }

        internal Exchange(Guid id, string name, string description) : base (id)
        {
            SetName(name);
            SetDescription(description);
        }

        [MemberNotNull(nameof(Name))]
        public Exchange SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), ExchangeConsts.MaxNameLength, ExchangeConsts.MinNameLength);
            return this;
        }

        [MemberNotNull(nameof(Description))]
        public Exchange SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), ExchangeConsts.MaxDescriptionLength, ExchangeConsts.MinDescriptionLength);
            return this;
        }
    }
}
