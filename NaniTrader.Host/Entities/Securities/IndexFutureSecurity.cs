using System.Diagnostics.CodeAnalysis;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NaniTrader.Entities.Securities
{
    public class IndexFutureSecurity : SecurityBase
    {
        // here for ef core
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
        private IndexFutureSecurity() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor.

        internal IndexFutureSecurity(Guid id, Guid parentId, IndexSecurity underlying, string name, string description)
            : base(id, parentId, name, description)
        {
            Underlying = underlying;
        }

        public IndexSecurity Underlying { get; internal set; }
    }
}
