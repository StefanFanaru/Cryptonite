using System.Collections.Generic;
using System.Threading.Tasks;
using Cryptonite.Infrastructure.Services.Portofolio.Dtos;

namespace Cryptonite.Infrastructure.Abstractions.Portofolio
{
    public interface IDistributionService
    {
        Task<List<DistributionItem>> BuildDistributionItems(string userId, string currency,
            Dictionary<string, decimal> currentCryptoCurrenciesValues);
    }
}