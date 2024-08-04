using AutoMapper;
using NaniTrader.Entities.Brokers;
using NaniTrader.Entities.Exchanges;
using NaniTrader.Entities.MarketData;
using NaniTrader.Entities.Securities;
using NaniTrader.Services.Brokers;
using NaniTrader.Services.Exchanges;
using NaniTrader.Services.MarketData;
using NaniTrader.Services.Securities;

namespace NaniTrader.ObjectMapping;

public class NaniTraderAutoMapperProfile : Profile
{
    public NaniTraderAutoMapperProfile()
    {
        /* Create your AutoMapper object mappings here */
        CreateMap<Exchange, ExchangeDto>();
        CreateMap<Exchange, ExchangeInListDto>();
        CreateMap<CreateUpdateExchangeDto, Exchange>();

        CreateMap<Broker, BrokerDto>();
        CreateMap<Broker, BrokerInListDto>();
        CreateMap<CreateUpdateBrokerDto, Broker>();

        CreateMap<MarketDataProvider, MarketDataProviderDto>();
        CreateMap<MarketDataProvider, MarketDataProviderInListDto>();
        CreateMap<CreateUpdateMarketDataProviderDto, MarketDataProvider>();

        CreateMap<EquitySecurity, EquitySecurityDto>();
        CreateMap<EquitySecurity, EquitySecurityInListDto>();
        CreateMap<CreateUpdateEquitySecurityDto, EquitySecurity>();

        CreateMap<EquityFutureSecurity, EquityFutureSecurityDto>();
        CreateMap<EquityFutureSecurity, EquityFutureSecurityInListDto>();
        CreateMap<CreateUpdateEquityFutureSecurityDto, EquityFutureSecurity>();

        CreateMap<EquityOptionSecurity, EquityOptionSecurityDto>();
        CreateMap<EquityOptionSecurity, EquityOptionSecurityInListDto>();
        CreateMap<CreateUpdateEquityOptionSecurityDto, EquityOptionSecurity>();

        CreateMap<IndexSecurity, IndexSecurityDto>();
        CreateMap<IndexSecurity, IndexSecurityInListDto>();
        CreateMap<CreateUpdateIndexSecurityDto, IndexSecurity>();

        CreateMap<IndexFutureSecurity, IndexFutureSecurityDto>();
        CreateMap<IndexFutureSecurity, IndexFutureSecurityInListDto>();
        CreateMap<CreateUpdateIndexFutureSecurityDto, IndexFutureSecurity>();

        CreateMap<IndexOptionSecurity, IndexOptionSecurityDto>();
        CreateMap<IndexOptionSecurity, IndexOptionSecurityInListDto>();
        CreateMap<CreateUpdateIndexOptionSecurityDto, IndexOptionSecurity>();
    }
}
