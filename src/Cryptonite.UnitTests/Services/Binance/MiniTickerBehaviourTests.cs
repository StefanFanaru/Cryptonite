using System.Collections.Generic;
using System.Linq;
using Cryptonite.Infrastructure.Helpers;
using Cryptonite.Infrastructure.Services.Binance.Apis.Dtos;
using Cryptonite.Infrastructure.Services.Binance.Sockets;
using Cryptonite.Infrastructure.Services.Binance.Sockets.Dtos;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Cryptonite.UnitTests.Services.Binance
{
    public class MiniTickerBehaviourTests
    {
        private const string Message =
            "[{\"e\":\"24hrTicker\",\"E\":1632500425713,\"s\":\"ETHUSDT\",\"p\":\"-0.00202300\",\"P\":\"-2.860\",\"w\":\"0.06872420\",\"x\":\"0.07073400\",\"c\":\"0.06870600\",\"Q\":\"0.22410000\",\"b\":\"0.06870200\",\"B\":\"7.80340000\",\"a\":\"0.06871800\",\"A\":\"0.41490000\",\"o\":\"0.07072900\",\"h\":\"0.07103400\",\"l\":\"0.06652500\",\"v\":\"141060.36860000\",\"q\":\"9694.26100355\",\"O\":1632414025694,\"C\":1632500425694,\"F\":297809150,\"L\":298090697,\"n\":281548}]";

        [Fact]
        public void On_message_received_updates_tickers()
        {
            var mediatorMock = new Mock<IMediator>();
            var binanceTickers = new BinanceTickers();
            var miniTickerBehaviour = new MiniTickerBehaviour(binanceTickers, mediatorMock.Object);
            var miniTickers = Message.FromJson<List<MiniTickerReceivedData>>();
            var miniTicker = miniTickers.First();
            miniTickerBehaviour.OnMessage(miniTickers);
            binanceTickers.GetCurrentMiniTickers().Single().Should().BeEquivalentTo(
                new KeyValuePair<string, MiniTickerData>(
                    "ETHUSDT",
                    new MiniTickerData
                    {
                        LastPrice = miniTicker.LastPrice
                    }));
        }

        [Fact]
        public void On_empty_message_received_returns()
        {
            var mediatorMock = new Mock<IMediator>();
            var binanceTickers = new BinanceTickers();
            var miniTickerBehaviour = new MiniTickerBehaviour(binanceTickers, mediatorMock.Object);
            binanceTickers.InitializeTickers(new List<TickerResponse>());
            miniTickerBehaviour.OnMessage(null);
            binanceTickers.GetCurrentMiniTickers().Should().HaveCount(0);
        }
    }
}