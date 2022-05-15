using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cryptonite.Infrastructure.Abstractions.Services.CurrencyLayer
{
    public interface ICurrencyLayerClient
    {
        Task<Dictionary<string, decimal>> RequestCurrenciesQuotes();
        Task<Dictionary<string, decimal>> RequestHistoricalCurrenciesQuotes(DateTime date);
    }
}