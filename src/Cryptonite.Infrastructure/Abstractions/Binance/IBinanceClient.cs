using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cryptonite.Core.Constants;
using Cryptonite.Infrastructure.Services.Binance.Apis.Dtos;

namespace Cryptonite.Infrastructure.Abstractions.Binance
{
    public interface IBinanceClient
    {
        Task<IEnumerable<TickerResponse>> GetTickers();

        Task<IEnumerable<IEnumerable<object>>> GetKline(string symbol, DateTimeOffset startTime, DateTimeOffset endTime,
            KlineInterval interval);
    }
}