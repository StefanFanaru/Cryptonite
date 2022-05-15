using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Infrastructure.Abstractions.Binance;
using Cryptonite.Infrastructure.Abstractions.Services.CurrencyLayer;
using Cryptonite.Infrastructure.Abstractions.UserSettingsService;
using Cryptonite.Infrastructure.CQRS;
using Cryptonite.Infrastructure.Data.Repositories;
using Cryptonite.Infrastructure.Queries.Portofolio.Results.Today;
using Cryptonite.Infrastructure.Services.Binance.Sockets.Dtos;
using Cryptonite.UnitTests.Helpers;
using FluentAssertions;
using Moq;
using Xunit;

namespace Cryptonite.UnitTests.Queries
{
    public class TodayResultQueryTests
    {
        private TodayResultQueryHandler CreateSut()
        {
            var repository = ServiceHelpers.CreateRepository<IPortofolioRepository, PortofolioRepository>();

            repository.IncreaseCryptocurrencyAmount(TestConstants.UserId, "ADA", 3).Wait();

            var currencyLayerServiceMock = new Mock<ICurrencyLayerService>();
            currencyLayerServiceMock.Setup(x => x.GetCurrentQuote(It.IsAny<string>())).ReturnsAsync(4);

            var userSettingsServiceMock = new Mock<IUserSettingsService>();
            userSettingsServiceMock.Setup(x => x.GetPreferredCurrency(TestConstants.UserId)).ReturnsAsync("RON");

            var binanceTickersMock = new Mock<IBinanceTickers>();
            binanceTickersMock.Setup(x => x.GetCurrentMiniTickers()).Returns(new Dictionary<string, MiniTickerData>
            {
                {
                    "ADAUSDT", new MiniTickerData
                    {
                        LastPrice = 4
                    }
                }
            });

            var binanceKlinesMock = new Mock<IBinanceKlines>();
            binanceKlinesMock.Setup(x => x.GetDayCloseQuote(It.IsAny<string>(), It.IsAny<DateTimeOffset>()))
                .ReturnsAsync(2m);

            return new TodayResultQueryHandler(userSettingsServiceMock.Object, currencyLayerServiceMock.Object,
                binanceTickersMock.Object, repository, binanceKlinesMock.Object);
        }

        [Fact]
        public async Task Calculates_today_result()
        {
            var sut = CreateSut();
            var result = await sut.Handle(new TodayResultQuery().WithUserId(TestConstants.UserId), new CancellationToken());
            var actual = result.Result;

            actual.Currency.Should().Be("RON");
            actual.TotalOutcome.Should().Be(24m);
            actual.Items.First().Should().BeEquivalentTo(new ResultItem
            {
                PriceChangePercent = 100,
                Outcome = 24m,
                Symbol = "ADA"
            });
        }
    }
}