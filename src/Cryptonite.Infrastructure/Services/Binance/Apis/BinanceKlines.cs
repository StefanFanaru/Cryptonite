using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cryptonite.Core.Constants;
using Cryptonite.Core.Helpers;
using Cryptonite.Infrastructure.Abstractions.Binance;
using Cryptonite.Infrastructure.Common;
using Microsoft.Extensions.Caching.Memory;

namespace Cryptonite.Infrastructure.Services.Binance.Apis
{
    public class BinanceKlines : IBinanceKlines
    {
        private readonly IBinanceClient _binanceClient;
        private readonly IMemoryCache _memoryCache;

        public BinanceKlines(IBinanceClient binanceClient, IMemoryCache memoryCache)
        {
            _binanceClient = binanceClient;
            _memoryCache = memoryCache;
        }

        public async Task<decimal> GetDayCloseQuote(string symbol, DateTimeOffset date)
        {
            var cacheKey = CacheKeys.KlineDayCloseQuote(symbol, date);
            if (_memoryCache.TryGetValue(cacheKey, out decimal quote))
            {
                return quote;
            }

            var kline = (await GetDayKline(symbol, date)).ToList();
            var result = Convert.ToDecimal(kline.First().ElementAt(4));
            _memoryCache.Set(cacheKey, result, TimeSpan.FromHours(1));
            return result;
        }

        private async Task<IEnumerable<IEnumerable<object>>> GetDayKline(string symbol, DateTimeOffset date)
        {
            var startTime = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0).ToDateTimeOffset();
            var endTime = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59).ToDateTimeOffset();

            return await _binanceClient.GetKline(symbol, startTime, endTime, KlineInterval.Day);
        }
    }
}