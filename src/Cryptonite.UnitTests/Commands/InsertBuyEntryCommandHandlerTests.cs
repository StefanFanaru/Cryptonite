using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Core.Common;
using Cryptonite.Core.Constants;
using Cryptonite.Core.Entities;
using Cryptonite.Infrastructure.Abstractions.Services.CurrencyLayer;
using Cryptonite.Infrastructure.Commands.BuyEntries.Insert;
using Cryptonite.Infrastructure.Data.Repositories;
using Cryptonite.Infrastructure.Services.CurrencyLayer;
using Cryptonite.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Cryptonite.UnitTests.Commands
{
    public class InsertBuyEntryCommandHandlerTests
    {
        private (InsertBuyEntryCommandHandler handler, IRepository repository) CreateSut()
        {
            var repository = ServiceHelpers.CreateRepository();

            var currencies = new Dictionary<string, decimal>
            {
                { "EUR", 0.853m },
                { "RON", 4.223m }
            };

            var currencyLayerClientMock = new Mock<ICurrencyLayerClient>();
            currencyLayerClientMock.Setup(x => x.RequestHistoricalCurrenciesQuotes(It.IsAny<DateTime>()))
                .ReturnsAsync(currencies);

            var currencyLayerService =
                new CurrencyLayerService(repository, currencyLayerClientMock.Object, ServiceHelpers.CreateMemoryCache());

            var portofolioRepository = ServiceHelpers.CreateRepository<IPortofolioRepository, PortofolioRepository>();

            return (new InsertBuyEntryCommandHandler(repository, currencyLayerService, portofolioRepository), repository);
        }

        private InsertBuyEntryCommand BuildCommand(string paymentCurrency, string bankAccountCurrency,
            decimal bankConversionMargin = 1.45m)
        {
            return new InsertBuyEntryCommand
            {
                UserId = TestConstants.UserId,
                BankConversionMargin = bankConversionMargin,
                PaidAmount = 200,
                PaymentCurrency = paymentCurrency,
                BoughtAt = TimeProvider.UtcNow.Date,
                BoughtAmount = 225.241m,
                BoughtCryptocurrency = CryptoniteConstants.BaseCryptoQuote,
                BankAccountCurrency = bankAccountCurrency
            };
        }

        [Fact]
        public async Task Inserts_new_buy_entry_with_different_currencies()
        {
            var (handler, repository) = CreateSut();
            await handler.Handle(BuildCommand("EUR", "RON"), new CancellationToken());

            var actual = await repository.Query<BuyEntry>()
                .FirstOrDefaultAsync(x => x.UserId == TestConstants.UserId);

            actual.Should().BeEquivalentTo(new BuyEntry
            {
                UserId = TestConstants.UserId,
                PaidAmount = 200,
                PaymentCurrency = "EUR",
                BoughtAt = TimeProvider.UtcNow.Date,
                BoughtAmount = 225.241m,
                BoughtCryptocurrency = CryptoniteConstants.BaseCryptoQuote
            }, options => options.Excluding(x => x.Id).Excluding(x => x.PaidUsd));

            Math.Round(actual.PaidUsd, 3).Should().Be(237.866m);
        }

        [Fact]
        public async Task Inserts_new_buy_entry_with_same_currency()
        {
            var (handler, repository) = CreateSut();
            await handler.Handle(BuildCommand("USD", "USD"), new CancellationToken());

            var actual = await repository.Query<BuyEntry>()
                .FirstOrDefaultAsync(x => x.UserId == TestConstants.UserId);

            actual.Should().BeEquivalentTo(new BuyEntry
            {
                UserId = TestConstants.UserId,
                PaidAmount = 200,
                PaymentCurrency = "USD",
                BoughtAt = TimeProvider.UtcNow.Date,
                BoughtAmount = 225.241m,
                BoughtCryptocurrency = CryptoniteConstants.BaseCryptoQuote,
                PaidUsd = 200
            }, options => options.Excluding(x => x.Id));
        }

        [Fact]
        public async Task Inserts_new_buy_entry_with_no_bank_margin()
        {
            var (handler, repository) = CreateSut();
            await handler.Handle(BuildCommand("EUR", "RON", 0), new CancellationToken());

            var actual = await repository.Query<BuyEntry>()
                .FirstOrDefaultAsync(x => x.UserId == TestConstants.UserId);

            actual.Should().BeEquivalentTo(new BuyEntry
            {
                UserId = TestConstants.UserId,
                PaidAmount = 200,
                PaymentCurrency = "EUR",
                BoughtAt = TimeProvider.UtcNow.Date,
                BoughtAmount = 225.241m,
                BoughtCryptocurrency = CryptoniteConstants.BaseCryptoQuote
            }, options => options.Excluding(x => x.Id).Excluding(x => x.PaidUsd));

            Math.Round(actual.PaidUsd, 3).Should().Be(234.467m);
        }
    }
}