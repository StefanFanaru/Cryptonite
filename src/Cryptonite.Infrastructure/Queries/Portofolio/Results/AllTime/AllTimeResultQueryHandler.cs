using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Core.Entities;
using Cryptonite.Infrastructure.Abstractions.Binance;
using Cryptonite.Infrastructure.Abstractions.Portofolio;
using Cryptonite.Infrastructure.Abstractions.Services.CurrencyLayer;
using Cryptonite.Infrastructure.Abstractions.UserSettingsService;
using Cryptonite.Infrastructure.CQRS.Operations;
using Cryptonite.Infrastructure.Data.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cryptonite.Infrastructure.Queries.Portofolio.Results.AllTime
{
    public class AllTimeResultQueryHandler : IRequestHandler<AllTimeResultQuery, IOperationResult<AllTimeResultQueryResult>>
    {
        private readonly IBinanceTickers _binanceTickers;
        private readonly ICurrencyLayerService _currencyLayerService;
        private readonly IDistributionService _distributionService;
        private readonly IRepository _repository;
        private readonly IUserSettingsService _userSettingsService;

        public AllTimeResultQueryHandler(
            IUserSettingsService userSettingsService,
            ICurrencyLayerService currencyLayerService,
            IBinanceTickers binanceTickers,
            IDistributionService distributionService,
            IRepository repository)
        {
            _userSettingsService = userSettingsService;
            _currencyLayerService = currencyLayerService;
            _binanceTickers = binanceTickers;
            _distributionService = distributionService;
            _repository = repository;
        }

        public async Task<IOperationResult<AllTimeResultQueryResult>> Handle(AllTimeResultQuery request,
            CancellationToken cancellationToken)
        {
            var preferredCurrency = await _userSettingsService.GetPreferredCurrency(request.UserId);
            var currentCryptoValues = _binanceTickers.GetCurrentCryptoCurrencyValues();
            var currentCurrencyQuote = await _currencyLayerService.GetCurrentQuote(preferredCurrency);

            var (currencyQuoteDifferential, totalPaidValue) =
                await CalculateCurrencyQuoteDifferential(request, cancellationToken, preferredCurrency, currentCurrencyQuote);

            var totalPortofolioValue = (await _distributionService.BuildDistributionItems(request.UserId, preferredCurrency,
                currentCryptoValues)).Sum(x => x.Value);

            return ResultBuilder.Ok(new AllTimeResultQueryResult
            {
                Currency = preferredCurrency,
                Result = currencyQuoteDifferential + totalPortofolioValue - totalPaidValue
            });
        }

        private async Task<(decimal currencyQuoteDifferential, decimal totalPaidValue)> CalculateCurrencyQuoteDifferential(AllTimeResultQuery request,
            CancellationToken cancellationToken,
            string preferredCurrency,
            decimal currentCurrencyQuote)
        {
            var buyEntries = await _repository.Query<BuyEntry>().Where(x => x.UserId == request.UserId)
                .Select(x => new { x.PaidUsd, x.BoughtAt }).ToListAsync(cancellationToken);

            var currencyQuoteDifferential = 0m;
            var totalPaidValue = 0m;
            foreach (var buyEntry in buyEntries)
            {
                var quoteOnDay = await _currencyLayerService.GetHistoricalQuote(preferredCurrency, buyEntry.BoughtAt);
                var paidAtDate = buyEntry.PaidUsd * quoteOnDay;
                var paidToday = buyEntry.PaidUsd * currentCurrencyQuote;
                currencyQuoteDifferential += paidToday - paidAtDate;
                totalPaidValue += paidAtDate;
            }

            return (currencyQuoteDifferential, totalPaidValue);
        }
    }
}