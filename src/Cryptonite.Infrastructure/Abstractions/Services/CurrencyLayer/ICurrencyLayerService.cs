using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cryptonite.Infrastructure.Abstractions.Services.CurrencyLayer
{
    public interface ICurrencyLayerService
    {
        Task<Dictionary<string, decimal>> GetCurrentQuotes();
        Task<decimal> GetHistoricalQuote(string currency, DateTime date);
        Task<decimal> GetCurrentQuote(string currency);
    }
}