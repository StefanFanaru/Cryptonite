using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Core.Constants;
using Cryptonite.Infrastructure.Abstractions.Binance;
using Cryptonite.Infrastructure.Abstractions.Portofolio;
using Cryptonite.Infrastructure.Abstractions.UserSettingsService;
using Cryptonite.Infrastructure.CQRS.Operations;
using MediatR;

namespace Cryptonite.Infrastructure.Queries.Portofolio.Distribution
{
    public class DistributionQueryHandler : IRequestHandler<DistributionQuery, IOperationResult<DistributionQueryResult>>
    {
        private readonly IBinanceTickers _binanceTickers;
        private readonly IDistributionService _distributionService;
        private readonly IUserSettingsService _userSettingsService;

        public DistributionQueryHandler(
            IUserSettingsService userSettingsService,
            IDistributionService distributionService,
            IBinanceTickers binanceTickers)
        {
            _userSettingsService = userSettingsService;
            _distributionService = distributionService;
            _binanceTickers = binanceTickers;
        }

        public async Task<IOperationResult<DistributionQueryResult>> Handle(DistributionQuery request,
            CancellationToken cancellationToken)
        {
            var preferredCurrency = await _userSettingsService.GetPreferredCurrency(request.UserId);
            var currentCryptoValues = _binanceTickers.GetCurrentCryptoCurrencyValues();
            var distributionItems = await _distributionService.BuildDistributionItems(request.UserId, preferredCurrency,
                currentCryptoValues);

            return ResultBuilder.Ok(new DistributionQueryResult
            {
                Currency = preferredCurrency,
                Items = distributionItems.Where(x => x.Symbol != CryptoniteConstants.BaseCryptoQuote).OrderByDescending(x => x.Value),
                TotalValue = distributionItems.Select(x => x.Value).Sum()
            });
        }
    }
}