using System.Collections.Generic;
using System.Threading.Tasks;
using Cryptonite.Core.Constants;
using Cryptonite.Infrastructure.Abstractions.Portofolio;
using Cryptonite.Infrastructure.Abstractions.Services.CurrencyLayer;
using Cryptonite.Infrastructure.Data.Repositories;
using Cryptonite.Infrastructure.Services.Portofolio.Dtos;

namespace Cryptonite.Infrastructure.Services.Portofolio
{
    public class DistributionService : IDistributionService
    {
        private readonly ICurrencyLayerService _currencyLayerService;
        private readonly IPortofolioRepository _portofolioRepository;

        public DistributionService(IPortofolioRepository portofolioRepository, ICurrencyLayerService currencyLayerService)
        {
            _portofolioRepository = portofolioRepository;
            _currencyLayerService = currencyLayerService;
        }

        public async Task<List<DistributionItem>> BuildDistributionItems(string userId, string currency,
            Dictionary<string, decimal> currentCryptoCurrenciesValues)
        {
            var portofolioCurrencies = await _portofolioRepository.RetrieveCurrenciesAsync(userId);
            var currencyQuote = await _currencyLayerService.GetCurrentQuote(currency);

            var distributionItems = portofolioCurrencies.Select(portofolioCurrency =>
            {
                var baseQuote = portofolioCurrency.Symbol == CryptoniteConstants.BaseCryptoQuote
                    ? 1
                    : currentCryptoCurrenciesValues[portofolioCurrency.Symbol];

                return new DistributionItem
                {
                    Symbol = portofolioCurrency.Symbol,
                    Value = baseQuote * portofolioCurrency.Amount * currencyQuote
                };
            }).ToList();

            return distributionItems;
        }
    }
}