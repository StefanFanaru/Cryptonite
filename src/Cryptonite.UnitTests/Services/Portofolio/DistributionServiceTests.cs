using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cryptonite.Infrastructure.Abstractions.Services.CurrencyLayer;
using Cryptonite.Infrastructure.Data.Repositories;
using Cryptonite.Infrastructure.Services.Portofolio;
using Cryptonite.UnitTests.Helpers;
using FluentAssertions;
using Moq;
using Xunit;

namespace Cryptonite.UnitTests.Services.Portofolio
{
    public class DistributionServiceTests
    {
        private DistributionService CreateSut()
        {
            var repository = ServiceHelpers.CreateRepository<IPortofolioRepository, PortofolioRepository>();

            repository.IncreaseCryptocurrencyAmount(TestConstants.UserId, "ADA", 3).Wait();
            repository.IncreaseCryptocurrencyAmount(TestConstants.UserId, "DOT", 3).Wait();

            var currencyLayerServiceMock = new Mock<ICurrencyLayerService>();
            currencyLayerServiceMock.Setup(x => x.GetCurrentQuote(It.IsAny<string>())).ReturnsAsync(4);

            return new DistributionService(repository, currencyLayerServiceMock.Object);
        }

        [Fact]
        public async Task Builds_distribution_items()
        {
            var sut = CreateSut();

            var actual = await sut.BuildDistributionItems(TestConstants.UserId, "EUR",
                new Dictionary<string, decimal> { { "ADA", 4m }, { "DOT", 3m } });

            var ada = actual.First(x => x.Symbol == "ADA");
            var dot = actual.First(x => x.Symbol == "DOT");
            ada.Value.Should().Be(48);
            dot.Value.Should().Be(36);
        }
    }
}