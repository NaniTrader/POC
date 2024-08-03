using Volo.Abp.Domain.Entities.Auditing;

namespace NaniTrader.Entities.Securities
{
    public class EquityFutureSecurity : SecurityBase
    {
        internal EquityFutureSecurity(Guid id, string name, string description) 
            : base(id, name, description)
        {
        }
    }
}
