using Volo.Abp.Domain.Entities.Auditing;

namespace NaniTrader.Entities.Securities
{
    public class EquitySecurity : SecurityBase
    {
        internal EquitySecurity(Guid id, string name, string description)
            : base(id, name, description)
        {
        }
    }
}
