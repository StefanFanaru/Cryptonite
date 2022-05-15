using System;
using System.Threading.Tasks;
using Cryptonite.Infrastructure.Abstractions.Binance;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cryptonite.IntegrationTests.ServicesTests
{
    public class BinanceTickersTests : IClassFixture<ApiTestFixture>
    {
        private readonly IBinanceSocket _binanceSocket;
        private readonly IBinanceTickers _binanceTickers;
        private readonly IServiceProvider _serviceProvider;

        public BinanceTickersTests(ApiTestFixture factory)
        {
            _serviceProvider = factory.Services;
            _binanceTickers = _serviceProvider.GetRequiredService<IBinanceTickers>();
            _binanceSocket = _serviceProvider.GetRequiredService<IBinanceSocket>();

            var binanceClient = _serviceProvider.GetRequiredService<IBinanceClient>();
            var binanceTickers = _serviceProvider.GetRequiredService<IBinanceTickers>();
            binanceTickers.InitializeTickers(binanceClient.GetTickers().Result);
        }

        [Fact]
        public void Tickers_are_initialized()
        {
            var tickers = _binanceTickers.GetCurrentMiniTickers();
            tickers.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Tickers_have_live_updates()
        {
            var initialTickers = _binanceTickers.GetCurrentMiniTickers();
            _binanceSocket.StartMiniTickerConnection();
            var initialTicker = initialTickers["ADAUSDT"];

            await Task.Delay(3000);

            var newTickers = _binanceTickers.GetCurrentMiniTickers();
            var newTicker = newTickers["ADAUSDT"];

            initialTicker.Should().NotBeEquivalentTo(newTicker);
            initialTickers.Values.Should().NotBeEquivalentTo(newTickers);

            foreach (var ticker in newTickers.Values)
            {
                ticker.LastPrice.Should().NotBe(0.0m);
            }
        }
    }
}