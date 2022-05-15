using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Cryptonite.Infrastructure.Abstractions.Binance;
using Cryptonite.Infrastructure.Services.Binance.Sockets.Dtos;
using JetBrains.Annotations;
using MediatR;
using Serilog;
using WebSocketSharp;

namespace Cryptonite.Infrastructure.Services.Binance.Sockets
{
    public class MiniTickerBehaviour : IMiniTickerBehaviour
    {
        private readonly IBinanceTickers _binanceTickers;
        private readonly IMediator _mediator;

        public MiniTickerBehaviour(IBinanceTickers binanceTickers, IMediator mediator)
        {
            _binanceTickers = binanceTickers;
            _mediator = mediator;
        }

        public void OnMessage(List<MiniTickerReceivedData> tickers)
        {
            if (tickers.Any())
            {
                _binanceTickers.UpdateMiniTickers(tickers);
                Task.Run(async () => await _mediator.Publish(new MiniTickersReceived()));
            }
        }

        [ExcludeFromCodeCoverage]
        public void OnOpen([CanBeNull] object sender, EventArgs eventArgs)
        {
            Log.Information("Binance MiniTicker socket has been opened");
        }

        [ExcludeFromCodeCoverage]
        public void OnClose([CanBeNull] object sender, EventArgs eventArgs)
        {
            Log.Information("Binance MiniTicker socket has been closed");
        }

        [ExcludeFromCodeCoverage]
        public void OnError([CanBeNull] object sender, ErrorEventArgs e)
        {
            Log.Error(e.Exception, e.Message);
        }
    }
}