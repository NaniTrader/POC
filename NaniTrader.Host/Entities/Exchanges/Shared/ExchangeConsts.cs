using System;
using System.Collections.Generic;
using System.Text;

namespace NaniTrader.Entities.Exchanges.Shared
{
    public static class ExchangeConsts
    {
        public const int MinNameLength = 1;
        public const int MaxNameLength = 256;

        public const int MinDescriptionLength = 1;
        public const int MaxDescriptionLength = 2000;

        public const int MinISINLength = 1;
        public const int MaxISINLength = 256;

        public const int MinTickerSymbolLength = 1;
        public const int MaxTickerSymbolLength = 256;
    }
}
