using System;
using System.Threading.Tasks;
using Cryptonite.Infrastructure.Abstractions.Binance;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cryptonite.IntegrationTests.ServicesTests
{
    public class BinanceKlinesTests : IClassFixture<ApiTestFixture>
    {
        private readonly IServiceProvider _serviceProvider;

        public BinanceKlinesTests(ApiTestFixture factory)
        {
            _serviceProvider = factory.Server.Services;
        }

        [Fact]
        public async Task Can_fetch_day_close_quote()
        {
            var client = _serviceProvider.GetRequiredService<IBinanceKlines>();
            var kline = await client.GetDayCloseQuote("ADAUSDT", new DateTime(2021, 10, 7));
            kline.Should().Be(2.27800000m);
        }
    }
}