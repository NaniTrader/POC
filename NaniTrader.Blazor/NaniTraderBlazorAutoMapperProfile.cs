using AutoMapper;
using NaniTrader.Services.Brokers;
using NaniTrader.Services.Exchanges;
using NaniTrader.Services.MarketData;
using NaniTrader.Services.Securities;

namespace NaniTrader;

public class NaniTraderBlazorAutoMapperProfile : Profile
{
    public NaniTraderBlazorAutoMapperProfile()
    {
        //Define your AutoMapper configuration here for the Blazor project.
        /* Create your AutoMapper object mappings here */

        CreateMap<BrokerDto, CreateUpdateBrokerDto>();
        CreateMap<ExchangeDto, CreateUpdateExchangeDto>();
        CreateMap<MarketDataProviderDto, CreateUpdateMarketDataProviderDto>();
        CreateMap<EquitySecurityDto, CreateUpdateEquitySecurityDto>();
    }
}
