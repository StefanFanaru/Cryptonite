using System;
using System.Threading.Tasks;

namespace Cryptonite.Infrastructure.Abstractions.Binance
{
    public interface IBinanceKlines
    {
        Task<decimal> GetDayCloseQuote(string symbol, DateTimeOffset date);
    }
}