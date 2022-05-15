using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cryptonite.Core.Entities;

namespace Cryptonite.Infrastructure.Data.Repositories
{
    public interface IPortofolioRepository
    {
        Task<Portofolio> RetrieveAsync(string userId);
        Task IncreaseCryptocurrencyAmount(string userId, string cryptocurrencySymbol, decimal amount, DateTime? time = null);
        Task DecreaseCryptocurrencyAmount(string userId, string cryptocurrencySymbol, decimal amount, DateTime? time = null);
        Task<List<PortofolioCryptocurrency>> RetrieveCurrenciesAsync(string userid);
    }
}