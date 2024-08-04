using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaniTrader.Services.Permissions
{
    public static class NaniTraderPermissions
    {
        public const string GroupName = "NaniTrader";

        public static class Exchanges
        {
            public const string Default = GroupName + ".Exchanges";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }

        public static class Brokers
        {
            public const string Default = GroupName + ".Brokers";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }

        public static class MarketDataProviders
        {
            public const string Default = GroupName + ".MarketDataProviders";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }

        public static class Securities
        {
            public const string Default = GroupName + ".Securities";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
    }
}
