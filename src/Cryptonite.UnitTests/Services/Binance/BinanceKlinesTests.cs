using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cryptonite.Core.Constants;
using Cryptonite.Infrastructure.Abstractions.Binance;
using Cryptonite.Infrastructure.Common;
using Cryptonite.Infrastructure.Helpers;
using Cryptonite.Infrastructure.Services.Binance.Apis;
using Cryptonite.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace Cryptonite.UnitTests.Services.Binance
{
    public class BinanceKlinesTests
    {
        private readonly IMemoryCache _cache;
        private Mock<IBinanceClient> _binanceClientMock;

        public BinanceKlinesTests()
        {
            _cache = ServiceHelpers.CreateMemoryCache();
        }

        private BinanceKlines CreateSut()
        {
            var klineResponseJson =
                "[[1633564800000,\"2.20900000\",\"2.37500000\",\"2.15000000\",\"2.27800000\",\"254708498.40000000\",1633651199999,\"579456296.88760000\",895423,\"126166143.70000000\",\"287274960.86860000\",\"0\"]]";
            _binanceClientMock = new Mock<IBinanceClient>();
            _binanceClientMock.Setup(x =>
                    x.GetKline(It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<KlineInterval>()))
                .ReturnsAsync(klineResponseJson.FromJson<IEnumerable<IEnumerable<object>>>());

            return new BinanceKlines(_binanceClientMock.Object, _cache);
        }

        [Fact]
        public async Task Fetches_day_close_quote_from_api_or_cache()
        {
            var sut = CreateSut();
            var date = DateTimeOffset.UtcNow.AddDays(-1);
            var symbol = "ADAUSDT";
            var current = await sut.GetDayCloseQuote(symbol, date);

            current.Should().Be(2.27800000m);

            _cache.TryGetValue(CacheKeys.KlineDayCloseQuote(symbol, date), out decimal cachedValue).Should().BeTrue();
            cachedValue.Should().Be(current);

            var newValue = await sut.GetDayCloseQuote(symbol, date);
            newValue.Should().Be(cachedValue);

            _binanceClientMock.Verify(x => x.GetKline(It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(),
                It.IsAny<KlineInterval>()), Times.Once);
        }
    }
}