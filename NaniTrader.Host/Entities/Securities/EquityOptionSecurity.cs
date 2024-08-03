using Volo.Abp.Domain.Entities.Auditing;

namespace NaniTrader.Entities.Securities
{
    public class EquityOptionSecurity : SecurityBase
    {
        internal EquityOptionSecurity(Guid id, string name, string description)
            : base(id, name, description)
        {
        }
    }
}
