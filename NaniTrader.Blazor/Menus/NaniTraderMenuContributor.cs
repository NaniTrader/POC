using NaniTrader.Blazor.Menus;
using NaniTrader.Localization;
using NaniTrader.MultiTenancy;
using NaniTrader.Services.Permissions;
using Volo.Abp.Account.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.Blazor;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.TenantManagement.Blazor.Navigation;
using Volo.Abp.UI.Navigation;

namespace NaniTrader.Menus;

public class NaniTraderMenuContributor : IMenuContributor
{
    private readonly IConfiguration _configuration;

    public NaniTraderMenuContributor(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
        else if (context.Menu.Name == StandardMenus.User)
        {
            await ConfigureUserMenuAsync(context);
        }
    }

    private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<NaniTraderResource>();

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                NaniTraderMenus.Home,
                l["Menu:Home"],
                "/",
                icon: "fas fa-home",
                order: 0
            )
        );

        var administration = context.Menu.GetAdministration();

        // NOTE: Enable if multitenancy desired
        //if (MultiTenancyConsts.IsEnabled)
        //{
        //    administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        //}
        //else
        //{
        //    administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        //}

        administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenus.GroupName, 3);

        if (await context.IsGrantedAsync(NaniTraderPermissions.Exchanges.Default))
        {
            context.Menu.AddItem(
            new ApplicationMenuItem(
                "Exchanges",
                l["Menu:Exchanges"],
                icon: "fa fa-book").AddItem(
                new ApplicationMenuItem(
                    "Exchanges.ListView",
                    l["Menu:Exchanges.ListView"],
                    url: "/exchanges")));
        }

        if (await context.IsGrantedAsync(NaniTraderPermissions.Brokers.Default))
        {
            context.Menu.AddItem(
            new ApplicationMenuItem(
                "Brokers",
                l["Menu:Brokers"],
                icon: "fa fa-book").AddItem(
                new ApplicationMenuItem(
                    "Brokers.ListView",
                    l["Menu:Brokers.ListView"],
                    url: "/brokers")));
        }

        if (await context.IsGrantedAsync(NaniTraderPermissions.MarketDataProviders.Default))
        {
            context.Menu.AddItem(
            new ApplicationMenuItem(
                "MarketDataProviders",
                l["Menu:MarketDataProviders"],
                icon: "fa fa-book").AddItem(
                new ApplicationMenuItem(
                    "MarketDataProviders.ListView",
                    l["Menu:MarketDataProviders.ListView"],
                    url: "/market-data-providers")));
        }

        if (await context.IsGrantedAsync(NaniTraderPermissions.Securities.Default))
        {
            var securitiesMenuItem = new ApplicationMenuItem(
                "Securities",
                l["Menu:Securities"],
                icon: "fa fa-book");

            var equitySecuritiesMenuItem = new ApplicationMenuItem(
                    "EquitySecurities.ListView",
                    l["Menu:EquitySecurities.ListView"],
                    url: "/equity-securities");

            securitiesMenuItem.AddItem(equitySecuritiesMenuItem);

            var equityFutureSecuritiesMenuItem = new ApplicationMenuItem(
                    "EquityFutureSecurities.ListView",
                    l["Menu:EquityFutureSecurities.ListView"],
                    url: "/equity-future-securities");

            securitiesMenuItem.AddItem(equityFutureSecuritiesMenuItem);

            var equityOptionSecuritiesMenuItem = new ApplicationMenuItem(
                    "EquityOptionSecurities.ListView",
                    l["Menu:EquityOptionSecurities.ListView"],
                    url: "/equity-option-securities");

            securitiesMenuItem.AddItem(equityOptionSecuritiesMenuItem);

            var indexSecuritiesMenuItem = new ApplicationMenuItem(
                    "IndexSecurities.ListView",
                    l["Menu:IndexSecurities.ListView"],
                    url: "/index-securities");

            securitiesMenuItem.AddItem(indexSecuritiesMenuItem);

            var indexFutureSecuritiesMenuItem = new ApplicationMenuItem(
                    "IndexFutureSecurities.ListView",
                    l["Menu:IndexFutureSecurities.ListView"],
                    url: "/index-future-securities");

            securitiesMenuItem.AddItem(indexFutureSecuritiesMenuItem);

            var indexOptionSecuritiesMenuItem = new ApplicationMenuItem(
                    "IndexOptionSecurities.ListView",
                    l["Menu:IndexOptionSecurities.ListView"],
                    url: "/index-option-securities");

            securitiesMenuItem.AddItem(indexOptionSecuritiesMenuItem);

            context.Menu.AddItem(securitiesMenuItem);
        }
    }

    private Task ConfigureUserMenuAsync(MenuConfigurationContext context)
    {
        var accountStringLocalizer = context.GetLocalizer<AccountResource>();

        var authServerUrl = _configuration["AuthServer:Authority"] ?? "";

        context.Menu.AddItem(new ApplicationMenuItem(
            "Account.Manage",
            accountStringLocalizer["MyAccount"],
            $"{authServerUrl.EnsureEndsWith('/')}Account/Manage?returnUrl={_configuration["App:SelfUrl"]}",
            icon: "fa fa-cog",
            order: 1000,
            null).RequireAuthenticated());

        return Task.CompletedTask;
    }
}
