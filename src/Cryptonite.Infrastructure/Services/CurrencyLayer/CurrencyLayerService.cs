using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cryptonite.Core.Common;
using Cryptonite.Core.Entities;
using Cryptonite.Core.Exceptions;
using Cryptonite.Infrastructure.Abstractions.Services.CurrencyLayer;
using Cryptonite.Infrastructure.Common;
using Cryptonite.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Cryptonite.Infrastructure.Services.CurrencyLayer
{
    public class CurrencyLayerService : ICurrencyLayerService
    {
        private readonly IMemoryCache _cache;
        private readonly ICurrencyLayerClient _currencyLayerClient;
        private readonly IRepository _repository;

        public CurrencyLayerService(IRepository repository, ICurrencyLayerClient currencyLayerClient, IMemoryCache cache)
        {
            _repository = repository;
            _currencyLayerClient = currencyLayerClient;
            _cache = cache;
        }

        public async Task<Dictionary<string, decimal>> GetCurrentQuotes()
        {
            if (_cache.TryGetValue(CacheKeys.Currencies, out Dictionary<string, decimal> currentCurrencies))
            {
                return currentCurrencies;
            }

            await FetchCurrenciesForTheDay();
            return await GetCurrentQuotes();
        }

        public async Task<decimal> GetCurrentQuote(string currency)
        {
            if (_cache.TryGetValue(CacheKeys.Currencies, out Dictionary<string, decimal> currentCurrencies))
            {
                return FindCurrencyQuote(currency, currentCurrencies);
            }

            await FetchCurrenciesForTheDay();
            return await GetCurrentQuote(currency);
        }

        public async Task<decimal> GetHistoricalQuote(string currency, DateTime date)
        {
            if (_cache.TryGetValue(CacheKeys.HistoricalCurrencyQuote(currency, date), out decimal historicalQuote))
            {
                return historicalQuote;
            }

            var query = _repository.Query<Currency>().Where(x => x.Date.Date == date.Date);

            if (await query.AnyAsync())
            {
                var quoteInDb = await query.Select(x => x.Currencies[currency]).SingleAsync();
                _cache.Set(CacheKeys.HistoricalCurrencyQuote(currency, date), quoteInDb, TimeSpan.FromHours(1));
                return quoteInDb;
            }

            var historicalCurrenciesQuotes = await _currencyLayerClient.RequestHistoricalCurrenciesQuotes(date);

            await _repository.InsertAsync(new Currency
            {
                Currencies = historicalCurrenciesQuotes,
                Date = date.Date
            });

            await _repository.SaveAsync();

            try
            {
                var result = FindCurrencyQuote(currency, historicalCurrenciesQuotes);
                _cache.Set(CacheKeys.HistoricalCurrencyQuote(currency, date), result, TimeSpan.FromHours(1));
                return result;
            }
            catch (BusinessException)
            {
                throw new BusinessException($"Could not find currency quote for {currency} on date {date.ToShortDateString()}");
            }
        }

        private static decimal FindCurrencyQuote(string currency, Dictionary<string, decimal> currencies)
        {
            if (currencies.TryGetValue(currency, out var currencyQuote))
            {
                return currencyQuote;
            }

            throw new BusinessException($"Could not find currency quote for {currencyQuote}");
        }

        private async Task FetchCurrenciesForTheDay()
        {
            var timeTillMidnight = DateTime.UtcNow.Date.AddDays(1.0) - DateTime.UtcNow;
            var query = _repository.Query<Currency>()
                .Where(x => x.Date == TimeProvider.UtcNow.Date);

            if (await query.AnyAsync())
            {
                var currentCurrencies = await query.Select(x => x.Currencies).SingleAsync();
                _cache.Set(CacheKeys.Currencies, currentCurrencies, timeTillMidnight);
                return;
            }

            var currenciesQuotes = await _currencyLayerClient.RequestCurrenciesQuotes();
            await _repository.InsertAsync(new Currency(currenciesQuotes));
            await _repository.SaveAsync();

            _cache.Set(CacheKeys.Currencies, currenciesQuotes, timeTillMidnight);
        }
    }
}