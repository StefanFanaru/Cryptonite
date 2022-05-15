using System.Collections.Generic;
using Cryptonite.Core.Constants;
using Cryptonite.Infrastructure.Abstractions.Binance;
using Cryptonite.Infrastructure.Services.Binance.Apis.Dtos;
using Cryptonite.Infrastructure.Services.Binance.Sockets.Dtos;

namespace Cryptonite.Infrastructure.Services.Binance.Sockets
{
    public class BinanceTickers : IBinanceTickers
    {
        private Dictionary<string, MiniTickerData> _miniTickers = new();

        public void UpdateMiniTickers(IEnumerable<MiniTickerReceivedData> miniTickers)
        {
            foreach (var miniTicker in miniTickers)
            {
                var ticker = new MiniTickerData
                {
                    LastPrice = miniTicker.LastPrice
                };

                if (!_miniTickers.ContainsKey(miniTicker.Symbol))
                {
                    _miniTickers.Add(miniTicker.Symbol, ticker);
                    continue;
                }

                _miniTickers[miniTicker.Symbol] = ticker;
            }
        }

        public void InitializeTickers(IEnumerable<TickerResponse> initialTickers)
        {
            _miniTickers = initialTickers.Where(x => x.Symbol.EndsWith(CryptoniteConstants.BaseCryptoQuote))
                .Where(x => x.LastPrice > 0.0m)
                .ToDictionary(x => x.Symbol, x => new MiniTickerData
                {
                    LastPrice = x.LastPrice
                });
        }

        public Dictionary<string, MiniTickerData> GetCurrentMiniTickers()
        {
            return _miniTickers;
        }

        public Dictionary<string, decimal> GetCurrentCryptoCurrencyValues()
        {
            return _miniTickers.Select(x => new KeyValuePair<string, decimal>(x.Key.Replace(
                    CryptoniteConstants.BaseCryptoQuote, string.Empty), x.Value.LastPrice))
                .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}