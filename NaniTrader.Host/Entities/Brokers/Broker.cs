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

namespace NaniTrader.Entities.Brokers
{
    public class Broker : FullAuditedAggregateRoot<Guid>
    {
        // here for ef core
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
        private Broker() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor.

        public string Name { get; private set; }
        public string Description { get; private set; }

        internal Broker(Guid id, string name, string description) : base (id)
        {
            SetName(name);
            SetDescription(description);
        }

        [MemberNotNull(nameof(Name))]
        public Broker SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), BrokerConsts.MaxNameLength, BrokerConsts.MinNameLength);
            return this;
        }

        [MemberNotNull(nameof(Description))]
        public Broker SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), BrokerConsts.MaxDescriptionLength, BrokerConsts.MinDescriptionLength);
            return this;
        }
    }
}
