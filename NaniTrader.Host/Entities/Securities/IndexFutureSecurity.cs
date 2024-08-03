using Volo.Abp.Domain.Entities.Auditing;

namespace NaniTrader.Entities.Securities
{
    public class IndexFutureSecurity : SecurityBase
    {
        internal IndexFutureSecurity(Guid id, string name, string description)
            : base(id, name, description)
        {
        }
    }
}
