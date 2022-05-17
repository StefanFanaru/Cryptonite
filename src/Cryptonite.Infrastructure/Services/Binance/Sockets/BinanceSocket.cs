using System.Security.Authentication;
using Cryptonite.Core.Constants;
using Cryptonite.Infrastructure.Abstractions.Binance;
using Cryptonite.Infrastructure.Services.Binance.Sockets.Dtos;
using Newtonsoft.Json.Linq;
using WebSocketSharp;

namespace Cryptonite.Infrastructure.Services.Binance.Sockets
{
    public class BinanceSocket : IBinanceSocket
    {
        private readonly IMiniTickerBehaviour _miniTickerBehaviour;
        private WebSocket _webSocket;

        public BinanceSocket(IMiniTickerBehaviour miniTickerBehaviour)
        {
            _miniTickerBehaviour = miniTickerBehaviour;
        }

        public void StartMiniTickerConnection()
        {
            _webSocket = new WebSocket("wss://stream.binance.com:9443/ws/!ticker@arr");
            _webSocket.SslConfiguration.EnabledSslProtocols = SslProtocols.Tls12;
            _webSocket.OnMessage += (_, args) =>
            {
                if (string.IsNullOrWhiteSpace(args.Data))
                {
                    return;
                }

                var jObject = JToken.Parse(args.Data);
                var tickers = jObject
                    .Where(c => c["s"].Value<string>().EndsWith(CryptoniteConstants.BaseCryptoQuote))
                    .Select(x => new MiniTickerReceivedData
                    {
                        Symbol = x["s"].Value<string>(),
                        LastPrice = x["c"].Value<decimal>()
                    }).ToList();

                _miniTickerBehaviour.OnMessage(tickers);
            };

            _webSocket.OnError += _miniTickerBehaviour.OnError;
            _webSocket.OnOpen += _miniTickerBehaviour.OnOpen;
            _webSocket.OnClose += _miniTickerBehaviour.OnClose;
            _webSocket.Connect();
        }

        public void StopMiniTickerConnection()
        {
            _webSocket?.Close();
        }

        public bool IsAlive()
        {
            return _webSocket?.IsAlive ?? false;
        }
    }
}