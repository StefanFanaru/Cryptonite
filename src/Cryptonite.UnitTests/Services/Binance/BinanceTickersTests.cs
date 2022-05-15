using System.Collections.Generic;
using System.Linq;
using Cryptonite.Infrastructure.Services.Binance.Apis.Dtos;
using Cryptonite.Infrastructure.Services.Binance.Sockets;
using Cryptonite.Infrastructure.Services.Binance.Sockets.Dtos;
using FluentAssertions;
using Xunit;

namespace Cryptonite.UnitTests.Services.Binance
{
    public class BinanceTickersTests
    {
        private BinanceTickers CreateSut()
        {
            var instance = new BinanceTickers();
            instance.InitializeTickers(new List<TickerResponse>
            {
                new()
                {
                    Symbol = "ETHUSDT",
                    LastPrice = 1.2m
                }
            });
            return instance;
        }

        [Fact]
        public void Tickers_are_updated()
        {
            var sut = CreateSut();
            sut.UpdateMiniTickers(new List<MiniTickerReceivedData>
            {
                new()
                {
                    Symbol = "ETHUSDT",
                    LastPrice = 2m
                }
            });

            var actual = sut.GetCurrentMiniTickers().Single();
            actual.Value.LastPrice.Should().Be(2m);
        }

        [Fact]
        public void Can_get_current_cryptoCurrencies_values()
        {
            var sut = CreateSut();
            var actual = sut.GetCurrentCryptoCurrencyValues();
            actual["ETH"].Should().Be(1.2m);
        }
    }
}