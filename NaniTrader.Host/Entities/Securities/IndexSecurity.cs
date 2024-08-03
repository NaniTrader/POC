using Volo.Abp.Domain.Entities.Auditing;

namespace NaniTrader.Entities.Securities
{
    public class IndexSecurity : SecurityBase
    {
        // here for ef core
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
        private IndexSecurity() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor.

        internal IndexSecurity(Guid id, Guid parentId, string name, string description)
            : base(id, parentId, name, description)
        {
        }
    }
}
