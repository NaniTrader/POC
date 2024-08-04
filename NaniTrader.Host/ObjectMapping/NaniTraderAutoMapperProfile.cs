using AutoMapper;
using NaniTrader.Entities.Exchanges;
using NaniTrader.Services.Exchanges;

namespace NaniTrader.ObjectMapping;

public class NaniTraderAutoMapperProfile : Profile
{
    public NaniTraderAutoMapperProfile()
    {
        /* Create your AutoMapper object mappings here */
        CreateMap<Exchange, ExchangeDto>();
        CreateMap<Exchange, ExchangeInListDto>();
        CreateMap<CreateUpdateExchangeDto, Exchange>();
    }
}
