using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NaniTrader.Entities.Brokers;
using NaniTrader.Entities.Exchanges;
using NaniTrader.Entities.MarketData;
using NaniTrader.Entities.Misc;
using NaniTrader.Entities.Securities;
using NaniTrader.Localization;
using NaniTrader.Pages;
using OpenIddict.Abstractions;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace NaniTrader.Data;

/* Creates initial data that is needed to property run the application
 * and make client-to-server communication possible.
 */
public class NaniTraderDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IConfiguration _configuration;
    private readonly IStringLocalizer<NaniTraderResource> L;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IRepository<Country, Guid> _countryRepository;
    private readonly IBrokerRepository _brokerRepository;
    private readonly IExchangeRepository _exchangeRepository;
    private readonly IMarketDataProviderRepository _marketDataProviderRepository;
    private readonly IEquitySecurityRepository _equitySecurityRepository;
    private readonly IEquityFutureSecurityRepository _equityFutureSecurityRepository;
    private readonly IEquityOptionSecurityRepository _equityOptionSecurityRepository;
    private readonly IIndexSecurityRepository _indexSecurityRepository;
    private readonly IIndexFutureSecurityRepository _indexFutureSecurityRepository;
    private readonly IIndexOptionSecurityRepository _indexOptionSecurityRepository;

    public NaniTraderDataSeedContributor(
        IConfiguration configuration,
        IStringLocalizer<NaniTraderResource> l,
        IGuidGenerator guidGenerator,
        IRepository<Country, Guid> countryRepository,
        IBrokerRepository brokerRepository,
        IExchangeRepository exchangeRepository,
        IMarketDataProviderRepository marketDataProviderRepository,
        IEquitySecurityRepository equitySecurityRepository,
        IEquityFutureSecurityRepository equityFutureSecurityRepository,
        IEquityOptionSecurityRepository equityOptionSecurityRepository,
        IIndexSecurityRepository indexSecurityRepository,
        IIndexFutureSecurityRepository indexFutureSecurityRepository,
        IIndexOptionSecurityRepository indexOptionSecurityRepository)
    {
        _configuration = configuration;
        L = l;
        _guidGenerator = guidGenerator;
        _countryRepository = countryRepository;
        _brokerRepository = brokerRepository;
        _exchangeRepository = exchangeRepository;
        _marketDataProviderRepository = marketDataProviderRepository;
        _equitySecurityRepository = equitySecurityRepository;
        _equityFutureSecurityRepository = equityFutureSecurityRepository;
        _equityOptionSecurityRepository = equityOptionSecurityRepository;
        _indexSecurityRepository = indexSecurityRepository;
        _indexFutureSecurityRepository = indexFutureSecurityRepository;
        _indexOptionSecurityRepository = indexOptionSecurityRepository;
    }

    [UnitOfWork]
    public virtual async Task SeedAsync(DataSeedContext context)
    {
        await CreateCountriesAsync();
        await CreateExchangesAsync();
        await CreateMarketDataProvidersAsync();
        await CreateEquitySecuritiesAsync();
        await CreateEquityFutureSecuritiesAsync();
        await CreateEquityOptionSecuritiesAsync();
        await CreateIndexSecuritiesAsync();
        await CreateIndexFutureSecuritiesAsync();
        await CreateIndexOptionSecuritiesAsync();
    }

    private async Task CreateCountriesAsync()
    {
        if (await _countryRepository.GetCountAsync() > 0)
        {
            return;
        }

        var countries = new List<Country>
            {
                new Country(_guidGenerator.Create(), "Afghanistan"),
                new Country(_guidGenerator.Create(), "Albania"),
                new Country(_guidGenerator.Create(), "Algeria"),
                new Country(_guidGenerator.Create(), "Andorra"),
                new Country(_guidGenerator.Create(), "Angola"),
                new Country(_guidGenerator.Create(), "Antigua and Barbuda"),
                new Country(_guidGenerator.Create(), "Argentina"),
                new Country(_guidGenerator.Create(), "Armenia"),
                new Country(_guidGenerator.Create(), "Australia"),
                new Country(_guidGenerator.Create(), "Austria"),
                new Country(_guidGenerator.Create(), "Azerbaijan"),
                new Country(_guidGenerator.Create(), "Bahamas"),
                new Country(_guidGenerator.Create(), "Bahrain"),
                new Country(_guidGenerator.Create(), "Bangladesh"),
                new Country(_guidGenerator.Create(), "Barbados"),
                new Country(_guidGenerator.Create(), "Belarus"),
                new Country(_guidGenerator.Create(), "Belgium"),
                new Country(_guidGenerator.Create(), "Belize"),
                new Country(_guidGenerator.Create(), "Benin"),
                new Country(_guidGenerator.Create(), "Bhutan"),
                new Country(_guidGenerator.Create(), "Bolivia"),
                new Country(_guidGenerator.Create(), "Bosnia and Herzegovina"),
                new Country(_guidGenerator.Create(), "Botswana"),
                new Country(_guidGenerator.Create(), "Brazil"),
                new Country(_guidGenerator.Create(), "Brunei"),
                new Country(_guidGenerator.Create(), "Bulgaria"),
                new Country(_guidGenerator.Create(), "Burkina Faso"),
                new Country(_guidGenerator.Create(), "Burundi"),
                new Country(_guidGenerator.Create(), "Côte d'Ivoire"),
                new Country(_guidGenerator.Create(), "Cabo Verde"),
                new Country(_guidGenerator.Create(), "Cambodia"),
                new Country(_guidGenerator.Create(), "Cameroon"),
                new Country(_guidGenerator.Create(), "Canada"),
                new Country(_guidGenerator.Create(), "Central African Republic"),
                new Country(_guidGenerator.Create(), "Chad"),
                new Country(_guidGenerator.Create(), "Chile"),
                new Country(_guidGenerator.Create(), "China"),
                new Country(_guidGenerator.Create(), "Colombia"),
                new Country(_guidGenerator.Create(), "Comoros"),
                new Country(_guidGenerator.Create(), "Congo (Congo-Brazzaville)"),
                new Country(_guidGenerator.Create(), "Costa Rica"),
                new Country(_guidGenerator.Create(), "Croatia"),
                new Country(_guidGenerator.Create(), "Cuba"),
                new Country(_guidGenerator.Create(), "Cyprus"),
                new Country(_guidGenerator.Create(), "Czechia (Czech Republic)"),
                new Country(_guidGenerator.Create(), "Democratic Republic of the Congo"),
                new Country(_guidGenerator.Create(), "Denmark"),
                new Country(_guidGenerator.Create(), "Djibouti"),
                new Country(_guidGenerator.Create(), "Dominica"),
                new Country(_guidGenerator.Create(), "Dominican Republic"),
                new Country(_guidGenerator.Create(), "Ecuador"),
                new Country(_guidGenerator.Create(), "Egypt"),
                new Country(_guidGenerator.Create(), "El Salvador"),
                new Country(_guidGenerator.Create(), "Equatorial Guinea"),
                new Country(_guidGenerator.Create(), "Eritrea"),
                new Country(_guidGenerator.Create(), "Estonia"),
                new Country(_guidGenerator.Create(), "Eswatini (formerly Swaziland)"),
                new Country(_guidGenerator.Create(), "Ethiopia"),
                new Country(_guidGenerator.Create(), "Fiji"),
                new Country(_guidGenerator.Create(), "Finland"),
                new Country(_guidGenerator.Create(), "France"),
                new Country(_guidGenerator.Create(), "Gabon"),
                new Country(_guidGenerator.Create(), "Gambia"),
                new Country(_guidGenerator.Create(), "Georgia"),
                new Country(_guidGenerator.Create(), "Germany"),
                new Country(_guidGenerator.Create(), "Ghana"),
                new Country(_guidGenerator.Create(), "Greece"),
                new Country(_guidGenerator.Create(), "Grenada"),
                new Country(_guidGenerator.Create(), "Guatemala"),
                new Country(_guidGenerator.Create(), "Guinea"),
                new Country(_guidGenerator.Create(), "Guinea-Bissau"),
                new Country(_guidGenerator.Create(), "Guyana"),
                new Country(_guidGenerator.Create(), "Haiti"),
                new Country(_guidGenerator.Create(), "Holy See"),
                new Country(_guidGenerator.Create(), "Honduras"),
                new Country(_guidGenerator.Create(), "Hungary"),
                new Country(_guidGenerator.Create(), "Iceland"),
                new Country(_guidGenerator.Create(), "India"),
                new Country(_guidGenerator.Create(), "Indonesia"),
                new Country(_guidGenerator.Create(), "Iran"),
                new Country(_guidGenerator.Create(), "Iraq"),
                new Country(_guidGenerator.Create(), "Ireland"),
                new Country(_guidGenerator.Create(), "Israel"),
                new Country(_guidGenerator.Create(), "Italy"),
                new Country(_guidGenerator.Create(), "Jamaica"),
                new Country(_guidGenerator.Create(), "Japan"),
                new Country(_guidGenerator.Create(), "Jordan"),
                new Country(_guidGenerator.Create(), "Kazakhstan"),
                new Country(_guidGenerator.Create(), "Kenya"),
                new Country(_guidGenerator.Create(), "Kiribati"),
                new Country(_guidGenerator.Create(), "Kuwait"),
                new Country(_guidGenerator.Create(), "Kyrgyzstan"),
                new Country(_guidGenerator.Create(), "Laos"),
                new Country(_guidGenerator.Create(), "Latvia"),
                new Country(_guidGenerator.Create(), "Lebanon"),
                new Country(_guidGenerator.Create(), "Lesotho"),
                new Country(_guidGenerator.Create(), "Liberia"),
                new Country(_guidGenerator.Create(), "Libya"),
                new Country(_guidGenerator.Create(), "Liechtenstein"),
                new Country(_guidGenerator.Create(), "Lithuania"),
                new Country(_guidGenerator.Create(), "Luxembourg"),
                new Country(_guidGenerator.Create(), "Madagascar"),
                new Country(_guidGenerator.Create(), "Malawi"),
                new Country(_guidGenerator.Create(), "Malaysia"),
                new Country(_guidGenerator.Create(), "Maldives"),
                new Country(_guidGenerator.Create(), "Mali"),
                new Country(_guidGenerator.Create(), "Malta"),
                new Country(_guidGenerator.Create(), "Marshall Islands"),
                new Country(_guidGenerator.Create(), "Mauritania"),
                new Country(_guidGenerator.Create(), "Mauritius"),
                new Country(_guidGenerator.Create(), "Mexico"),
                new Country(_guidGenerator.Create(), "Micronesia"),
                new Country(_guidGenerator.Create(), "Moldova"),
                new Country(_guidGenerator.Create(), "Monaco"),
                new Country(_guidGenerator.Create(), "Mongolia"),
                new Country(_guidGenerator.Create(), "Montenegro"),
                new Country(_guidGenerator.Create(), "Morocco"),
                new Country(_guidGenerator.Create(), "Mozambique"),
                new Country(_guidGenerator.Create(), "Myanmar (formerly Burma)"),
                new Country(_guidGenerator.Create(), "Namibia"),
                new Country(_guidGenerator.Create(), "Nauru"),
                new Country(_guidGenerator.Create(), "Nepal"),
                new Country(_guidGenerator.Create(), "Netherlands"),
                new Country(_guidGenerator.Create(), "New Zealand"),
                new Country(_guidGenerator.Create(), "Nicaragua"),
                new Country(_guidGenerator.Create(), "Niger"),
                new Country(_guidGenerator.Create(), "Nigeria"),
                new Country(_guidGenerator.Create(), "North Korea"),
                new Country(_guidGenerator.Create(), "North Macedonia"),
                new Country(_guidGenerator.Create(), "Norway"),
                new Country(_guidGenerator.Create(), "Oman"),
                new Country(_guidGenerator.Create(), "Pakistan"),
                new Country(_guidGenerator.Create(), "Palau"),
                new Country(_guidGenerator.Create(), "Palestine State"),
                new Country(_guidGenerator.Create(), "Panama"),
                new Country(_guidGenerator.Create(), "Papua New Guinea"),
                new Country(_guidGenerator.Create(), "Paraguay"),
                new Country(_guidGenerator.Create(), "Peru"),
                new Country(_guidGenerator.Create(), "Philippines"),
                new Country(_guidGenerator.Create(), "Poland"),
                new Country(_guidGenerator.Create(), "Portugal"),
                new Country(_guidGenerator.Create(), "Qatar"),
                new Country(_guidGenerator.Create(), "Romania"),
                new Country(_guidGenerator.Create(), "Russia"),
                new Country(_guidGenerator.Create(), "Rwanda"),
                new Country(_guidGenerator.Create(), "Saint Kitts and Nevis"),
                new Country(_guidGenerator.Create(), "Saint Lucia"),
                new Country(_guidGenerator.Create(), "Saint Vincent and the Grenadines"),
                new Country(_guidGenerator.Create(), "Samoa"),
                new Country(_guidGenerator.Create(), "San Marino"),
                new Country(_guidGenerator.Create(), "Sao Tome and Principe"),
                new Country(_guidGenerator.Create(), "Saudi Arabia"),
                new Country(_guidGenerator.Create(), "Senegal"),
                new Country(_guidGenerator.Create(), "Serbia"),
                new Country(_guidGenerator.Create(), "Seychelles"),
                new Country(_guidGenerator.Create(), "Sierra Leone"),
                new Country(_guidGenerator.Create(), "Singapore"),
                new Country(_guidGenerator.Create(), "Slovakia"),
                new Country(_guidGenerator.Create(), "Slovenia"),
                new Country(_guidGenerator.Create(), "Solomon Islands"),
                new Country(_guidGenerator.Create(), "Somalia"),
                new Country(_guidGenerator.Create(), "South Africa"),
                new Country(_guidGenerator.Create(), "South Korea"),
                new Country(_guidGenerator.Create(), "South Sudan"),
                new Country(_guidGenerator.Create(), "Spain"),
                new Country(_guidGenerator.Create(), "Sri Lanka"),
                new Country(_guidGenerator.Create(), "Sudan"),
                new Country(_guidGenerator.Create(), "Suriname"),
                new Country(_guidGenerator.Create(), "Sweden"),
                new Country(_guidGenerator.Create(), "Switzerland"),
                new Country(_guidGenerator.Create(), "Syria"),
                new Country(_guidGenerator.Create(), "Tajikistan"),
                new Country(_guidGenerator.Create(), "Tanzania"),
                new Country(_guidGenerator.Create(), "Thailand"),
                new Country(_guidGenerator.Create(), "Timor-Leste"),
                new Country(_guidGenerator.Create(), "Togo"),
                new Country(_guidGenerator.Create(), "Tonga"),
                new Country(_guidGenerator.Create(), "Trinidad and Tobago"),
                new Country(_guidGenerator.Create(), "Tunisia"),
                new Country(_guidGenerator.Create(), "Turkey"),
                new Country(_guidGenerator.Create(), "Turkmenistan"),
                new Country(_guidGenerator.Create(), "Tuvalu"),
                new Country(_guidGenerator.Create(), "Uganda"),
                new Country(_guidGenerator.Create(), "Ukraine"),
                new Country(_guidGenerator.Create(), "United Arab Emirates"),
                new Country(_guidGenerator.Create(), "United Kingdom"),
                new Country(_guidGenerator.Create(), "United States of America"),
                new Country(_guidGenerator.Create(), "Uruguay"),
                new Country(_guidGenerator.Create(), "Uzbekistan"),
                new Country(_guidGenerator.Create(), "Vanuatu"),
                new Country(_guidGenerator.Create(), "Venezuela"),
                new Country(_guidGenerator.Create(), "Vietnam"),
                new Country(_guidGenerator.Create(), "Yemen"),
                new Country(_guidGenerator.Create(), "Zambia"),
                new Country(_guidGenerator.Create(), "Zimbabwe")
            };

        await _countryRepository.InsertManyAsync(countries, autoSave: true);
    }

    private async Task CreateMarketDataProvidersAsync()
    {
        if (await _marketDataProviderRepository.GetCountAsync() > 0)
        {
            return;
        }

        var marketDataProviders = new List<MarketDataProvider>
        {
            new MarketDataProvider(_guidGenerator.Create(), "NSE_BhavCopy", "NSE BhavCopy"),
            new MarketDataProvider(_guidGenerator.Create(), "BSE_BhavCopy", "BSE BhavCopy")
        };

        await _marketDataProviderRepository.InsertManyAsync(marketDataProviders, autoSave: true);
    }

    private async Task CreateExchangesAsync()
    {
        if (await _exchangeRepository.GetCountAsync() > 0)
        {
            return;
        }

        var exchanges = new List<Exchange>
        {
            new Exchange(_guidGenerator.Create(), "NSE", "National Stock Exchange"),
            new Exchange(_guidGenerator.Create(), "BSE", "Bombay Stock Exchange")
        };

        await _exchangeRepository.InsertManyAsync(exchanges, autoSave: true);
    }

    private async Task CreateEquitySecuritiesAsync()
    {
        if (await _equitySecurityRepository.GetCountAsync() > 0)
        {
            return;
        }

        var nseExchange = await _exchangeRepository.FindByNameAsync("NSE");
        var bseExchange = await _exchangeRepository.FindByNameAsync("BSE");

        if (nseExchange?.Id != null && bseExchange?.Id != null)
        {
            var nseHDFC = new EquitySecurity(_guidGenerator.Create(), nseExchange.Id, "HDFC", "HDFC Bank");
            var nseSBIN = new EquitySecurity(_guidGenerator.Create(), nseExchange.Id , "SBIN", "SBI Bank");
            var bseHDFC = new EquitySecurity(_guidGenerator.Create(), bseExchange.Id , "HDFC", "HDFC Bank");
            var bseKotak = new EquitySecurity(_guidGenerator.Create(), bseExchange.Id , "Kotak", "Kotak Bank");
            await _equitySecurityRepository.InsertManyAsync(new List<EquitySecurity>() { nseHDFC, nseSBIN, bseHDFC, bseKotak }, autoSave: true);
        };
    }

    private async Task CreateEquityFutureSecuritiesAsync()
    {
        if (await _equityFutureSecurityRepository.GetCountAsync() > 0)
        {
            return;
        }

        var nseExchange = await _exchangeRepository.FindByNameAsync("NSE");
        var bseExchange = await _exchangeRepository.FindByNameAsync("BSE");

        var nseHDFC = await _equitySecurityRepository.FindByParentIdAndNameAsync(nseExchange?.Id ?? default, "HDFC");
        var nseSBIN = await _equitySecurityRepository.FindByParentIdAndNameAsync(nseExchange?.Id ?? default, "SBIN");
        var bseHDFC = await _equitySecurityRepository.FindByParentIdAndNameAsync(bseExchange?.Id ?? default, "HDFC");
        var bseKotak = await _equitySecurityRepository.FindByParentIdAndNameAsync(bseExchange?.Id ?? default, "Kotak");

        if (nseHDFC != null && nseExchange?.Id != null)
        {
            var hdfcFut1000 = new EquityFutureSecurity(_guidGenerator.Create(), nseExchange.Id, nseHDFC, "HDFCFUT1000", "HDFC Bank 1000 Future");
            var hdfcFut2000 = new EquityFutureSecurity(_guidGenerator.Create(), nseExchange.Id, nseHDFC, "HDFCFUT2000", "HDFC Bank 2000 Future");
            await _equityFutureSecurityRepository.InsertManyAsync(new List<EquityFutureSecurity>() { hdfcFut1000, hdfcFut2000 }, autoSave: true);
        }

        if (nseSBIN != null && nseExchange?.Id != null)
        {
            var sbinFut1000 = new EquityFutureSecurity(_guidGenerator.Create(), nseExchange.Id, nseSBIN, "SBINFUT1000", "SBI Bank 1000 Future");
            var sbinFut2000 = new EquityFutureSecurity(_guidGenerator.Create(), nseExchange.Id, nseSBIN, "SBINFUT2000", "SBI Bank 2000 Future");
            await _equityFutureSecurityRepository.InsertManyAsync(new List<EquityFutureSecurity>() { sbinFut1000, sbinFut2000 }, autoSave: true);
        }

        if (bseHDFC != null && bseExchange?.Id != null)
        {
            var hdfcFut1000 = new EquityFutureSecurity(_guidGenerator.Create(), bseExchange.Id, bseHDFC, "HDFCFUT1000", "HDFC Bank 1000 Future");
            var hdfcFut2000 = new EquityFutureSecurity(_guidGenerator.Create(), bseExchange.Id, bseHDFC, "HDFCFUT2000", "HDFC Bank 2000 Future");
            await _equityFutureSecurityRepository.InsertManyAsync(new List<EquityFutureSecurity>() { hdfcFut1000, hdfcFut2000 }, autoSave: true);
        }

        if (bseKotak != null && bseExchange?.Id != null)
        {
            var bseKotakFut1000 = new EquityFutureSecurity(_guidGenerator.Create(), bseExchange.Id, bseKotak, "KOTAKFUT1000", "KOTAK Bank 1000 Future");
            var bseKotakFut2000 = new EquityFutureSecurity(_guidGenerator.Create(), bseExchange.Id, bseKotak, "KOTAKFUT2000", "KOTAK Bank 2000 Future");
            await _equityFutureSecurityRepository.InsertManyAsync(new List<EquityFutureSecurity>() { bseKotakFut1000, bseKotakFut2000 }, autoSave: true);
        }
    }

    private async Task CreateEquityOptionSecuritiesAsync()
    {
        if (await _equityOptionSecurityRepository.GetCountAsync() > 0)
        {
            return;
        }

        var nseExchange = await _exchangeRepository.FindByNameAsync("NSE");
        var bseExchange = await _exchangeRepository.FindByNameAsync("BSE");

        var nseHDFC = await _equitySecurityRepository.FindByParentIdAndNameAsync(nseExchange?.Id ?? default, "HDFC");
        var nseSBIN = await _equitySecurityRepository.FindByParentIdAndNameAsync(nseExchange?.Id ?? default, "SBIN");
        var bseHDFC = await _equitySecurityRepository.FindByParentIdAndNameAsync(bseExchange?.Id ?? default, "HDFC");
        var bseKotak = await _equitySecurityRepository.FindByParentIdAndNameAsync(bseExchange?.Id ?? default, "Kotak");

        if (nseHDFC != null && nseExchange?.Id != null)
        {
            var hdfcOpt1000 = new EquityOptionSecurity(_guidGenerator.Create(), nseExchange.Id, nseHDFC, "HDFCOPT1000", "HDFC Bank 1000 Option");
            var hdfcOpt2000 = new EquityOptionSecurity(_guidGenerator.Create(), nseExchange.Id, nseHDFC, "HDFCOPT2000", "HDFC Bank 2000 Option");
            await _equityOptionSecurityRepository.InsertManyAsync(new List<EquityOptionSecurity>() { hdfcOpt1000, hdfcOpt2000 }, autoSave: true);
        }

        if (nseSBIN != null && nseExchange?.Id != null)
        {
            var sbinOpt1000 = new EquityOptionSecurity(_guidGenerator.Create(), nseExchange.Id, nseSBIN, "SBINOPT1000", "SBI Bank 1000 Option");
            var sbinOpt2000 = new EquityOptionSecurity(_guidGenerator.Create(), nseExchange.Id, nseSBIN, "SBINOPT2000", "SBI Bank 2000 Option");
            await _equityOptionSecurityRepository.InsertManyAsync(new List<EquityOptionSecurity>() { sbinOpt1000, sbinOpt2000 }, autoSave: true);
        }

        if (bseHDFC != null && bseExchange?.Id != null)
        {
            var hdfcOpt1000 = new EquityOptionSecurity(_guidGenerator.Create(), bseExchange.Id, bseHDFC, "HDFCOPT1000", "HDFC Bank 1000 Option");
            var hdfcOpt2000 = new EquityOptionSecurity(_guidGenerator.Create(), bseExchange.Id, bseHDFC, "HDFCOPT2000", "HDFC Bank 2000 Option");
            await _equityOptionSecurityRepository.InsertManyAsync(new List<EquityOptionSecurity>() { hdfcOpt1000, hdfcOpt2000 }, autoSave: true);
        }

        if (bseKotak != null && bseExchange?.Id != null)
        {
            var bseKotakOpt1000 = new EquityOptionSecurity(_guidGenerator.Create(), bseExchange.Id, bseKotak, "KOTAKOPT1000", "KOTAK Bank 1000 Option");
            var bseKotakOpt2000 = new EquityOptionSecurity(_guidGenerator.Create(), bseExchange.Id, bseKotak, "KOTAKOPT2000", "KOTAK Bank 2000 Option");
            await _equityOptionSecurityRepository.InsertManyAsync(new List<EquityOptionSecurity>() { bseKotakOpt1000, bseKotakOpt2000 }, autoSave: true);
        }
    }

    private async Task CreateIndexSecuritiesAsync()
    {
        if (await _indexSecurityRepository.GetCountAsync() > 0)
        {
            return;
        }

        var nseExchange = await _exchangeRepository.FindByNameAsync("NSE");
        var bseExchange = await _exchangeRepository.FindByNameAsync("BSE");

        if (nseExchange?.Id != null && bseExchange?.Id != null)
        {
            var nifty = new IndexSecurity(_guidGenerator.Create(), nseExchange.Id, "NIFTY", "NIFTY");
            var bankNifty = new IndexSecurity(_guidGenerator.Create(), nseExchange.Id, "BANKNIFTY", "NIFTY BANK");
            var sensex = new IndexSecurity(_guidGenerator.Create(), bseExchange.Id, "SENSEX", "SENSEX");
            var bankex = new IndexSecurity(_guidGenerator.Create(), bseExchange.Id, "BANKEX", "SENSEX BANK");
            await _indexSecurityRepository.InsertManyAsync(new List<IndexSecurity>() { nifty, bankNifty, sensex, bankex }, autoSave: true);
        };
    }

    private async Task CreateIndexFutureSecuritiesAsync()
    {
        if (await _indexFutureSecurityRepository.GetCountAsync() > 0)
        {
            return;
        }

        var nseExchange = await _exchangeRepository.FindByNameAsync("NSE");
        var bseExchange = await _exchangeRepository.FindByNameAsync("BSE");

        var nifty = await _indexSecurityRepository.FindByParentIdAndNameAsync(nseExchange?.Id ?? default, "NIFTY");
        var bankNifty = await _indexSecurityRepository.FindByParentIdAndNameAsync(nseExchange?.Id ?? default, "BANKNIFTY");
        var sensex = await _indexSecurityRepository.FindByParentIdAndNameAsync(bseExchange?.Id ?? default, "SENSEX");
        var bankex = await _indexSecurityRepository.FindByParentIdAndNameAsync(bseExchange?.Id ?? default, "BANKEX");

        if (nifty != null && nseExchange?.Id != null)
        {
            var niftyFut1000 = new IndexFutureSecurity(_guidGenerator.Create(), nseExchange.Id, nifty, "NIFTYFUT1000", "NIFTY 1000 Future");
            var niftyFut2000 = new IndexFutureSecurity(_guidGenerator.Create(), nseExchange.Id, nifty, "NIFTYFUT2000", "NIFTY 2000 Future");
            await _indexFutureSecurityRepository.InsertManyAsync(new List<IndexFutureSecurity>() { niftyFut1000, niftyFut2000 }, autoSave: true);
        }

        if (bankNifty != null && nseExchange?.Id != null)
        {
            var bankNiftyFut1000 = new IndexFutureSecurity(_guidGenerator.Create(), nseExchange.Id, bankNifty, "BANKNIFTYFUT1000", "Bank Nifty 1000 Future");
            var bankNiftyFut2000 = new IndexFutureSecurity(_guidGenerator.Create(), nseExchange.Id, bankNifty, "BANKNIFTYFUT2000", "Bank Nifty 2000 Future");
            await _indexFutureSecurityRepository.InsertManyAsync(new List<IndexFutureSecurity>() { bankNiftyFut1000, bankNiftyFut2000 }, autoSave: true);
        }

        if (sensex != null && bseExchange?.Id != null)
        {
            var sensexFut1000 = new IndexFutureSecurity(_guidGenerator.Create(), bseExchange.Id, sensex, "SENSEXFUT1000", "SENSEX 1000 Future");
            var sensexFut2000 = new IndexFutureSecurity(_guidGenerator.Create(), bseExchange.Id, sensex, "SENSEXFUT2000", "SENSEX 2000 Future");
            await _indexFutureSecurityRepository.InsertManyAsync(new List<IndexFutureSecurity>() { sensexFut1000, sensexFut2000 }, autoSave: true);
        }

        if (bankex != null && bseExchange?.Id != null)
        {
            var bankexFut1000 = new IndexFutureSecurity(_guidGenerator.Create(), bseExchange.Id, bankex, "BANKEXFUT1000", "Sensex Bank 1000 Future");
            var bankexFut2000 = new IndexFutureSecurity(_guidGenerator.Create(), bseExchange.Id, bankex, "BANKEXFUT2000", "Sensex Bank 2000 Future");
            await _indexFutureSecurityRepository.InsertManyAsync(new List<IndexFutureSecurity>() { bankexFut1000, bankexFut2000 }, autoSave: true);
        }
    }

    private async Task CreateIndexOptionSecuritiesAsync()
    {
        if (await _indexOptionSecurityRepository.GetCountAsync() > 0)
        {
            return;
        }

        var nseExchange = await _exchangeRepository.FindByNameAsync("NSE");
        var bseExchange = await _exchangeRepository.FindByNameAsync("BSE");

        var nifty = await _indexSecurityRepository.FindByParentIdAndNameAsync(nseExchange?.Id ?? default, "NIFTY");
        var bankNifty = await _indexSecurityRepository.FindByParentIdAndNameAsync(nseExchange?.Id ?? default, "BANKNIFTY");
        var sensex = await _indexSecurityRepository.FindByParentIdAndNameAsync(bseExchange?.Id ?? default, "SENSEX");
        var bankex = await _indexSecurityRepository.FindByParentIdAndNameAsync(bseExchange?.Id ?? default, "BANKEX");

        if (nifty != null && nseExchange?.Id != null)
        {
            var niftyOpt1000 = new IndexOptionSecurity(_guidGenerator.Create(), nseExchange.Id, nifty, "NIFTYOPT1000", "NIFTY 1000 Option");
            var niftyOpt2000 = new IndexOptionSecurity(_guidGenerator.Create(), nseExchange.Id, nifty, "NIFTYOPT2000", "NIFTY 2000 Option");
            await _indexOptionSecurityRepository.InsertManyAsync(new List<IndexOptionSecurity>() { niftyOpt1000, niftyOpt2000 }, autoSave: true);
        }

        if (bankNifty != null && nseExchange?.Id != null)
        {
            var bankNiftyOpt1000 = new IndexOptionSecurity(_guidGenerator.Create(), nseExchange.Id, bankNifty, "BANKNIFTYOPT1000", "Bank Nifty 1000 Option");
            var bankNiftyOpt2000 = new IndexOptionSecurity(_guidGenerator.Create(), nseExchange.Id, bankNifty, "BANKNIFTYOPT2000", "Bank Nifty 2000 Option");
            await _indexOptionSecurityRepository.InsertManyAsync(new List<IndexOptionSecurity>() { bankNiftyOpt1000, bankNiftyOpt2000 }, autoSave: true);
        }

        if (sensex != null && bseExchange?.Id != null)
        {
            var sensexOpt1000 = new IndexOptionSecurity(_guidGenerator.Create(), bseExchange.Id, sensex, "SENSEXOPT1000", "SENSEX 1000 Option");
            var sensexOpt2000 = new IndexOptionSecurity(_guidGenerator.Create(), bseExchange.Id, sensex, "SENSEXOPT2000", "SENSEX 2000 Option");
            await _indexOptionSecurityRepository.InsertManyAsync(new List<IndexOptionSecurity>() { sensexOpt1000, sensexOpt2000 }, autoSave: true);
        }

        if (bankex != null && bseExchange?.Id != null)
        {
            var bankexOpt1000 = new IndexOptionSecurity(_guidGenerator.Create(), bseExchange.Id, bankex, "BANKEXOPT1000", "Sensex Bank 1000 Option");
            var bankexOpt2000 = new IndexOptionSecurity(_guidGenerator.Create(), bseExchange.Id, bankex, "BANKEXOPT2000", "Sensex Bank 2000 Option");
            await _indexOptionSecurityRepository.InsertManyAsync(new List<IndexOptionSecurity>() { bankexOpt1000, bankexOpt2000 }, autoSave: true);
        }
    }
}
