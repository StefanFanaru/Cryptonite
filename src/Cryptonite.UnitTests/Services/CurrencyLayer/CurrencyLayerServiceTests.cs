using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cryptonite.Core.Common;
using Cryptonite.Core.Entities;
using Cryptonite.Core.Exceptions;
using Cryptonite.Infrastructure.Abstractions.Services.CurrencyLayer;
using Cryptonite.Infrastructure.Common;
using Cryptonite.Infrastructure.Data.Repositories;
using Cryptonite.Infrastructure.Services.CurrencyLayer;
using Cryptonite.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace Cryptonite.UnitTests.Services.CurrencyLayer
{
    public class CurrencyLayerServiceTests
    {
        private readonly IMemoryCache _cache;

        private readonly Dictionary<string, decimal> _currencies = new()
        {
            { "AED", 3.673042m },
            { "AFN", 87.950404m },
            { "ALL", 103.650403m }
        };

        private readonly Mock<ICurrencyLayerClient> _currencyLayerClientMock;
        private readonly IRepository _repository;

        public CurrencyLayerServiceTests()
        {
            _repository = ServiceHelpers.CreateRepository();
            _cache = ServiceHelpers.CreateMemoryCache();
            _currencyLayerClientMock = new Mock<ICurrencyLayerClient>();
        }

        private CurrencyLayerService CreateSut(Mock<ICurrencyLayerClient> currencyLayerClientMock = null)
        {
            if (currencyLayerClientMock != null)
            {
                return new CurrencyLayerService(_repository, _currencyLayerClientMock.Object, _cache);
            }

            _currencyLayerClientMock.Setup(x => x.RequestCurrenciesQuotes())
                .ReturnsAsync(_currencies);
            _currencyLayerClientMock.Setup(x => x.RequestHistoricalCurrenciesQuotes(It.IsAny<DateTime>()))
                .ReturnsAsync(_currencies);

            return new CurrencyLayerService(_repository, _currencyLayerClientMock.Object, _cache);
        }

        [Fact]
        public async Task Fetches_currencies_when_no_currencies_persisted_for_today()
        {
            var sut = CreateSut();
            var current = await sut.GetCurrentQuotes();

            current.Should().BeEquivalentTo(_currencies);
            var lastCurrency = await _repository.Query<Currency>().OrderByDescending(x => x.Date).FirstAsync();

            lastCurrency.Date.Date.Should().Be(TimeProvider.UtcNow.Date);
            lastCurrency.Currencies.Should().BeEquivalentTo(_currencies);
            _cache.TryGetValue(CacheKeys.Currencies, out var currencies).Should().BeTrue();
            currencies.Should().BeEquivalentTo(_currencies);
            _currencyLayerClientMock.Verify(x => x.RequestCurrenciesQuotes(), Times.Once);
        }

        [Fact]
        public async Task Does_not_fetch_currencies_when_already_persisted()
        {
            var sut = CreateSut();
            var alreadyPersistedCurrencies = new Dictionary<string, decimal> { { "AED", 1.1m } };
            await _repository.InsertAsync(new Currency(alreadyPersistedCurrencies));
            await _repository.SaveAsync();

            var current = await sut.GetCurrentQuotes();

            current.Should().BeEquivalentTo(alreadyPersistedCurrencies);
            var lastCurrency = await _repository.Query<Currency>().OrderByDescending(x => x.Date).FirstAsync();
            lastCurrency.Currencies.Should().BeEquivalentTo(alreadyPersistedCurrencies);
            _cache.TryGetValue(CacheKeys.Currencies, out var currencies).Should().BeTrue();
            currencies.Should().BeEquivalentTo(alreadyPersistedCurrencies);
            _currencyLayerClientMock.Verify(x => x.RequestCurrenciesQuotes(), Times.Never);
        }

        [Fact]
        public async Task FindCurrencyQuote_throws_exception_when_not_found()
        {
            var sut = CreateSut();
            var date = new DateTime(2021, 09, 25);
            await sut.Invoking(x => x.GetHistoricalQuote("ABC", date)).Should().ThrowAsync<BusinessException>();
        }

        [Fact]
        public async Task Can_get_current_currencies_quotes()
        {
            var sut = CreateSut();
            var current = await sut.GetCurrentQuotes();
            current.Should().BeEquivalentTo(_currencies);
            _currencyLayerClientMock.Verify(x => x.RequestCurrenciesQuotes(), Times.Once);
        }

        [Fact]
        public async Task Can_get_historical_currencies_quotes()
        {
            var sut = CreateSut();
            var date = new DateTime(2021, 09, 25);
            var historicalQuote = await sut.GetHistoricalQuote("AED", date);
            historicalQuote.Should().Be(3.673042m);
            _currencyLayerClientMock.Verify(x => x.RequestHistoricalCurrenciesQuotes(date), Times.Once);
        }

        [Fact]
        public async Task Fetches_historical_currencies_when_no_currencies_persisted_for_the_date()
        {
            var sut = CreateSut();
            var date = new DateTime(2021, 09, 25);
            var historicalQuote = await sut.GetHistoricalQuote("AED", date);

            historicalQuote.Should().Be(3.673042m);
            var persistedQuote = await _repository.Query<Currency>().FirstAsync(x => x.Date.Date == date.Date);

            persistedQuote.Date.Date.Should().Be(date.Date);
            persistedQuote.Currencies.Should().BeEquivalentTo(_currencies);
            _currencyLayerClientMock.Verify(x => x.RequestHistoricalCurrenciesQuotes(date), Times.Once);
        }

        [Fact]
        public async Task Does_not_fetch_historical_currencies_when_already_persisted()
        {
            var sut = CreateSut();
            var date = new DateTime(2021, 09, 25);
            var alreadyPersistedCurrencies = new Dictionary<string, decimal> { { "AED", 1.1m } };
            await _repository.InsertAsync(new Currency
            {
                Currencies = alreadyPersistedCurrencies,
                Date = date.Date
            });
            await _repository.SaveAsync();

            var historicalQuote = await sut.GetHistoricalQuote("AED", date);
            historicalQuote.Should().Be(1.1m);
            _currencyLayerClientMock.Verify(x => x.RequestHistoricalCurrenciesQuotes(date), Times.Never);
        }

        [Fact]
        public async Task Can_get_specific_currency_quote()
        {
            var sut = CreateSut();
            var actual = await sut.GetCurrentQuote("AED");
            actual.Should().Be(3.673042m);
            _currencyLayerClientMock.Verify(x => x.RequestCurrenciesQuotes(), Times.Once);
        }
    }
}