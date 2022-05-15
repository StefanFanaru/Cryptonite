using System;
using System.Linq;
using System.Threading.Tasks;
using Cryptonite.Infrastructure.Abstractions.Binance;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cryptonite.IntegrationTests.ServicesTests
{
    public class BinanceClientTests : IClassFixture<ApiTestFixture>
    {
        private readonly IServiceProvider _serviceProvider;

        public BinanceClientTests(ApiTestFixture factory)
        {
            _serviceProvider = factory.Server.Services;
        }

        [Fact]
        public async Task Can_fetch_tickers()
        {
            var client = _serviceProvider.GetRequiredService<IBinanceClient>();
            var tickers = await client.GetTickers();
            tickers.ToList().Count.Should().BeGreaterThan(0);
        }
    }
}