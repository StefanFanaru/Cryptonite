using System.Collections.Generic;
using Cryptonite.Infrastructure.Services.Binance.Apis.Dtos;
using Cryptonite.Infrastructure.Services.Binance.Sockets.Dtos;

namespace Cryptonite.Infrastructure.Abstractions.Binance
{
    public interface IBinanceTickers
    {
        void UpdateMiniTickers(IEnumerable<MiniTickerReceivedData> miniTickers);
        void InitializeTickers(IEnumerable<TickerResponse> initialTickers);
        Dictionary<string, MiniTickerData> GetCurrentMiniTickers();
        Dictionary<string, decimal> GetCurrentCryptoCurrencyValues();
    }
}