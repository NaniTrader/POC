﻿using System.Diagnostics.CodeAnalysis;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NaniTrader.Entities.Securities
{
    public class EquityOptionSecurity : SecurityBase
    {
        // here for ef core
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
        private EquityOptionSecurity() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor.

        internal EquityOptionSecurity(Guid id, Guid parentId, EquitySecurity underlying, string name, string description)
             : base(id, parentId, name, description)
        {
            Underlying = underlying;
        }

        public EquitySecurity Underlying { get; internal set; }
    }
}
