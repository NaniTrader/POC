using NaniTrader.Services.Misc.Shared;

namespace NaniTrader.Services.Misc
{
    public class UlidGeneratorService : NaniTraderAppService, IUlidGeneratorService
    {
        public Ulid Create()
        {
            return Ulid.NewUlid();
        }
    }
}
