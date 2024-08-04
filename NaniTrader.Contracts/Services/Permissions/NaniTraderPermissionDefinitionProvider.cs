using NaniTrader.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NaniTrader.Services.Permissions
{
    public class NaniTraderPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var naniTraderGroup = context.AddGroup(NaniTraderPermissions.GroupName, L("Permission:NaniTrader"));

            var exchangesPermission = naniTraderGroup.AddPermission(NaniTraderPermissions.Exchanges.Default, L("Permission:Exchanges"));
            exchangesPermission.AddChild(NaniTraderPermissions.Exchanges.Create, L("Permission:Exchanges.Create"));
            exchangesPermission.AddChild(NaniTraderPermissions.Exchanges.Edit, L("Permission:Exchanges.Edit"));
            exchangesPermission.AddChild(NaniTraderPermissions.Exchanges.Delete, L("Permission:Exchanges.Delete"));

            var brokersPermission = naniTraderGroup.AddPermission(NaniTraderPermissions.Brokers.Default, L("Permission:Brokers"));
            brokersPermission.AddChild(NaniTraderPermissions.Brokers.Create, L("Permission:Brokers.Create"));
            brokersPermission.AddChild(NaniTraderPermissions.Brokers.Edit, L("Permission:Brokers.Edit"));
            brokersPermission.AddChild(NaniTraderPermissions.Exchanges.Delete, L("Permission:Brokers.Delete"));

            var marketDataProvidersPermission = naniTraderGroup.AddPermission(NaniTraderPermissions.MarketDataProviders.Default, L("Permission:MarketDataProviders"));
            marketDataProvidersPermission.AddChild(NaniTraderPermissions.MarketDataProviders.Create, L("Permission:MarketDataProviders.Create"));
            marketDataProvidersPermission.AddChild(NaniTraderPermissions.MarketDataProviders.Edit, L("Permission:MarketDataProviders.Edit"));
            marketDataProvidersPermission.AddChild(NaniTraderPermissions.MarketDataProviders.Delete, L("Permission:MarketDataProviders.Delete"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<NaniTraderResource>(name);
        }
    }
}
