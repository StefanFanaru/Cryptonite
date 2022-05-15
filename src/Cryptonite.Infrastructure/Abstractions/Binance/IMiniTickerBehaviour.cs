using System;
using System.Collections.Generic;
using Cryptonite.Infrastructure.Services.Binance.Sockets.Dtos;
using JetBrains.Annotations;
using WebSocketSharp;

namespace Cryptonite.Infrastructure.Abstractions.Binance
{
    public interface IMiniTickerBehaviour
    {
        void OnMessage(List<MiniTickerReceivedData> tickers);
        void OnOpen([CanBeNull] object sender, EventArgs eventArgs);
        void OnClose([CanBeNull] object sender, EventArgs eventArgs);
        void OnError([CanBeNull] object sender, ErrorEventArgs e);
    }
}