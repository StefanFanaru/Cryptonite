using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Core.Entities;
using Cryptonite.Infrastructure.Abstractions.Binance;
using Cryptonite.Infrastructure.Abstractions.Portofolio;
using Cryptonite.Infrastructure.Abstractions.Services.CurrencyLayer;
using Cryptonite.Infrastructure.Abstractions.UserSettingsService;
using Cryptonite.Infrastructure.CQRS;
using Cryptonite.Infrastructure.Queries.Portofolio.Results.AllTime;
using Cryptonite.Infrastructure.Services.Portofolio.Dtos;
using Cryptonite.UnitTests.Helpers;
using FluentAssertions;
using Moq;
using Xunit;

namespace Cryptonite.UnitTests.Queries
{
    public class AllTimeResultQueryTests
    {
        private async Task<AllTimeResultQueryHandler> CreateSut(decimal historicalQuote)
        {
            var repository = ServiceHelpers.CreateRepository();
            await repository.InsertAsync(new BuyEntry
            {
                UserId = TestConstants.UserId,
                PaidUsd = 5,
                BoughtCryptocurrency = "USDT",
                PaymentCurrency = "EUR"
            });

            await repository.SaveAsync();

            var currencyLayerServiceMock = new Mock<ICurrencyLayerService>();
            currencyLayerServiceMock.Setup(x => x.GetCurrentQuote(It.IsAny<string>())).ReturnsAsync(4);
            currencyLayerServiceMock.Setup(x => x.GetHistoricalQuote(It.IsAny<string>(), It.IsAny<DateTime>()))
                .ReturnsAsync(historicalQuote);

            var userSettingsServiceMock = new Mock<IUserSettingsService>();
            userSettingsServiceMock.Setup(x => x.GetPreferredCurrency(TestConstants.UserId)).ReturnsAsync("RON");

            var binanceTickersMock = new Mock<IBinanceTickers>();
            binanceTickersMock.Setup(x => x.GetCurrentCryptoCurrencyValues()).Returns(new Dictionary<string, decimal>());

            var distributionServiceMock = new Mock<IDistributionService>();
            distributionServiceMock.Setup(x =>
                    x.BuildDistributionItems(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, decimal>>()))
                .ReturnsAsync(new List<DistributionItem>
                {
                    new()
                    {
                        Symbol = "ADA",
                        Value = 50
                    }
                });

            return new AllTimeResultQueryHandler(userSettingsServiceMock.Object, currencyLayerServiceMock.Object,
                binanceTickersMock.Object, distributionServiceMock.Object, repository);
        }

        [Theory]
        [InlineData(5, 25)]
        [InlineData(3, 35)]
        [InlineData(12, -10)]
        public async Task Calculates_all_time_result(decimal historicalQuote, decimal expected)
        {
            var sut = await CreateSut(historicalQuote);
            var result = await sut.Handle(new AllTimeResultQuery().WithUserId(TestConstants.UserId), new CancellationToken());
            var actual = result.Result;

            actual.Currency.Should().Be("RON");
            actual.Result.Should().Be(expected);
        }
    }
}