using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Infrastructure.Abstractions.Services.CurrencyLayer;
using Cryptonite.Infrastructure.Queries.Currencies.CurrenciesList;
using FluentAssertions;
using Moq;
using Xunit;

namespace Cryptonite.UnitTests.Queries
{
    public class CurrenciesListQueryTests
    {
        public CurrenciesListQueryHandler CreateSut()
        {
            var currencyLayerServiceMock = new Mock<ICurrencyLayerService>();
            currencyLayerServiceMock.Setup(x => x.GetCurrentQuotes()).ReturnsAsync(
                new Dictionary<string, decimal>
                {
                    { "EUR", 1.1m },
                    { "RON", 1.1m }
                });

            return new CurrenciesListQueryHandler(currencyLayerServiceMock.Object);
        }

        [Fact]
        public async Task Returns_currencies_names()
        {
            var sut = CreateSut();
            var actual = await sut.Handle(new CurrenciesListQuery(), new CancellationToken());

            actual.Result.Should().BeEquivalentTo(new List<string> { "EUR", "RON" });
        }
    }
}