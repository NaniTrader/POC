using Volo.Abp.Domain.Entities.Auditing;

namespace NaniTrader.Entities.Securities
{
    public class IndexSecurity : SecurityBase
    {
        internal IndexSecurity(Guid id, string name, string description)
            : base(id, name, description)
        {
        }
    }
}
