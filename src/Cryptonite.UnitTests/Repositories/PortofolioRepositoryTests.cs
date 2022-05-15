using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cryptonite.Core.Common;
using Cryptonite.Core.Entities;
using Cryptonite.Core.Exceptions;
using Cryptonite.Infrastructure.Data.Repositories;
using Cryptonite.UnitTests.Helpers;
using FluentAssertions;
using Xunit;

namespace Cryptonite.UnitTests.Repositories
{
    public class PortofolioRepositoryTests
    {
        public static IPortofolioRepository CreateSut()
        {
            return ServiceHelpers.CreateRepository<IPortofolioRepository, PortofolioRepository>();
        }

        [Fact]
        public async Task Adds_cryptocurrency()
        {
            var sut = CreateSut();

            await sut.IncreaseCryptocurrencyAmount(TestConstants.UserId, "ADA", 1.1m);

            var actual = await sut.RetrieveAsync(TestConstants.UserId);
            actual.Transactions.Should().Be(1);
            actual.LastTransactionAt.Should().BeCloseTo(TimeProvider.UtcNow, 500);

            var expectedCryptocurrencies = new List<PortofolioCryptocurrency>
            {
                new()
                {
                    Symbol = "ADA",
                    Amount = 1.1m,
                    PortofolioId = actual.Id
                }
            };

            var actualCurrencies = await sut.RetrieveCurrenciesAsync(TestConstants.UserId);
            actualCurrencies.Should().BeEquivalentTo(expectedCryptocurrencies, options => options
                .Excluding(x => x.InsertedAt));

            actualCurrencies.First().InsertedAt.Should().BeCloseTo(TimeProvider.UtcNow, 500);
        }

        [Fact]
        public async Task Updates_cryptocurrency()
        {
            var sut = CreateSut();

            await sut.IncreaseCryptocurrencyAmount(TestConstants.UserId, "ADA", 1.1m);
            await Task.Delay(500);
            await sut.IncreaseCryptocurrencyAmount(TestConstants.UserId, "ADA", 2.2m);

            var actual = await sut.RetrieveAsync(TestConstants.UserId);
            actual.Transactions.Should().Be(2);
            actual.LastTransactionAt.Should().BeCloseTo(TimeProvider.UtcNow, 500);

            var expectedCryptocurrencies = new List<PortofolioCryptocurrency>
            {
                new()
                {
                    Symbol = "ADA",
                    Amount = 3.3m,
                    PortofolioId = actual.Id
                }
            };

            var actualCurrencies = await sut.RetrieveCurrenciesAsync(TestConstants.UserId);
            actualCurrencies.Should().BeEquivalentTo(expectedCryptocurrencies, options => options
                .Excluding(x => x.InsertedAt)
                .Excluding(x => x.UpdatedAt));

            actualCurrencies.First().InsertedAt.Should().BeCloseTo(TimeProvider.UtcNow.AddMilliseconds(-500), 500);
            actualCurrencies.First().UpdatedAt.Should().BeCloseTo(TimeProvider.UtcNow, 500);
        }

        [Fact]
        public async Task Deletes_no_amount_cryptocurrency()
        {
            var sut = CreateSut();

            await sut.IncreaseCryptocurrencyAmount(TestConstants.UserId, "ADA", 1.1m);
            await sut.IncreaseCryptocurrencyAmount(TestConstants.UserId, "DOT", 1.2m);
            await sut.DecreaseCryptocurrencyAmount(TestConstants.UserId, "DOT", 1.2m);

            var actual = await sut.RetrieveAsync(TestConstants.UserId);
            actual.Transactions.Should().Be(3);
            actual.LastTransactionAt.Should().BeCloseTo(TimeProvider.UtcNow, 500);

            var expectedCryptocurrencies = new List<PortofolioCryptocurrency>
            {
                new()
                {
                    Symbol = "ADA",
                    Amount = 1.1m,
                    PortofolioId = actual.Id
                }
            };

            var actualCurrencies = await sut.RetrieveCurrenciesAsync(TestConstants.UserId);
            actualCurrencies.Should().BeEquivalentTo(expectedCryptocurrencies, options => options
                .Excluding(x => x.InsertedAt));

            actualCurrencies.First().InsertedAt.Should().BeCloseTo(TimeProvider.UtcNow, 500);
        }

        [Fact]
        public async Task Throws_when_decreasing_non_existent_currency()
        {
            var sut = CreateSut();
            await sut.Invoking(x => x.DecreaseCryptocurrencyAmount(TestConstants.UserId, "DOT", 1.2m)).Should()
                .ThrowAsync<BusinessException>();
        }
    }
}