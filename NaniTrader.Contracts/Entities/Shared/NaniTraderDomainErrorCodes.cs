namespace NaniTrader.Entities.Shared
{
    public static class NaniTraderDomainErrorCodes
    {
        /* You can add your business exception error codes here, as constants */
        public const string ExchangeAlreadyExists = "NaniTrader:00001";
        public const string MarketDataProviderAlreadyExists = "NaniTrader:00002";
        public const string BhavCopyAlreadyExists = "NaniTrader:00003";
        public const string BrokerAlreadyExists = "NaniTrader:00004";
        public const string SecurityAlreadyExists = "NaniTrader:00005";
        public const string SecurityNotFound = "NaniTrader:00006";
    }
}
