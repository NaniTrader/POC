using System.Diagnostics.CodeAnalysis;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NaniTrader.Entities.Securities
{
    public class IndexOptionSecurity : SecurityBase
    {
        // here for ef core
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
        private IndexOptionSecurity() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor.

        internal IndexOptionSecurity(Guid id, Guid parentId, IndexSecurity underlying, string name, string description)
            : base(id, parentId, name, description)
        {
            SetUnderlying(underlying);
        }

        [MemberNotNull(nameof(Underlying))]
        public IndexOptionSecurity SetUnderlying(IndexSecurity underlying)
        {
            Underlying = Check.NotNull(underlying, nameof(underlying));
            return this;
        }

        public IndexSecurity Underlying { get; private set; }
    }
}
