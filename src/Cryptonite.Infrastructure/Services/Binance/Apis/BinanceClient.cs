using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Cryptonite.Core.Constants;
using Cryptonite.Infrastructure.Abstractions.Binance;
using Cryptonite.Infrastructure.Helpers;
using Cryptonite.Infrastructure.Services.Binance.Apis.Dtos;

namespace Cryptonite.Infrastructure.Services.Binance.Apis
{
    public class BinanceClient : IBinanceClient
    {
        public async Task<IEnumerable<TickerResponse>> GetTickers()
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync("https://api.binance.com/api/v3/ticker/24hr");
            return response.FromJson<IEnumerable<TickerResponse>>();
        }

        public async Task<IEnumerable<IEnumerable<object>>> GetKline(string symbol, DateTimeOffset startTime, DateTimeOffset endTime,
            KlineInterval interval)
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync(
                $"https://api.binance.com/api/v3/klines?symbol={symbol}&interval={interval.Value}&startTime={startTime.ToUnixTimeMilliseconds()}&endTime={endTime.ToUnixTimeMilliseconds()}");
            return response.FromJson<IEnumerable<IEnumerable<object>>>();
        }
    }
}