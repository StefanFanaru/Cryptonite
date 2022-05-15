using System;

namespace Cryptonite.Infrastructure.Common
{
    public static class CacheKeys
    {
        public const string Currencies = nameof(Currencies);
        public const string PreferredCurrency = nameof(PreferredCurrency);

        public static string KlineDayCloseQuote(string symbol, DateTimeOffset date)
        {
            return $"KlineCloseQuote-{symbol}-{date.ToUnixTimeSeconds()}";
        }

        public static string KlineDayAverage(string symbol, DateTimeOffset date)
        {
            return $"KlineDayAverage-{symbol}-{date.ToUnixTimeSeconds()}";
        }

        public static string HistoricalCurrencyQuote(string currency, DateTimeOffset date)
        {
            return $"HistoricalCurrencyQuote-{currency}-{date.Date:d}";
        }

        public static string Portofolio(string userId)
        {
            return $"Portofolio-{userId}";
        }

        public static string PortofolioCurrencies(string userId)
        {
            return $"Portofolio-{userId}";
        }
    }
}