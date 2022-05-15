using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Infrastructure.Abstractions.Binance;
using Cryptonite.Infrastructure.Queries.CryptoData.Symbols;
using Cryptonite.Infrastructure.Services.Binance.Sockets.Dtos;
using FluentAssertions;
using Moq;
using Xunit;

namespace Cryptonite.UnitTests.Queries
{
    public class SymbolsQueryTests
    {
        private readonly Dictionary<string, MiniTickerData> _miniTickerData = new()
        {
            {
                "BTCUSDT", new MiniTickerData()
            },
            {
                "ADAUSDT", new MiniTickerData()
            }
        };

        public SymbolsQueryHandler CreateSut()
        {
            var binanceTickersMock = new Mock<IBinanceTickers>();
            binanceTickersMock.Setup(x => x.GetCurrentMiniTickers()).Returns(_miniTickerData);

            return new SymbolsQueryHandler(binanceTickersMock.Object);
        }

        [Fact]
        public async Task Returns_symbols_without_base_quote_suffix()
        {
            var sut = CreateSut();
            var actual = await sut.Handle(new SymbolsQuery(), new CancellationToken());

            actual.Result.Should().BeEquivalentTo(new List<string> { "BTC", "ADA", "USDT" });
        }
    }
}