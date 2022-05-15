using System;
using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Core.Constants;
using Cryptonite.Infrastructure.Abstractions.Binance;
using Cryptonite.Infrastructure.Abstractions.Services.CurrencyLayer;
using Cryptonite.Infrastructure.Abstractions.UserSettingsService;
using Cryptonite.Infrastructure.CQRS.Operations;
using Cryptonite.Infrastructure.Data.Repositories;
using MediatR;

namespace Cryptonite.Infrastructure.Queries.Portofolio.Results.Today
{
    public class TodayResultQueryHandler : IRequestHandler<TodayResultQuery, IOperationResult<TodayResultResponse>>
    {
        private readonly IBinanceKlines _binanceKlines;
        private readonly IBinanceTickers _binanceTickers;
        private readonly ICurrencyLayerService _currencyLayerService;
        private readonly IPortofolioRepository _portofolioRepository;
        private readonly IUserSettingsService _userSettingsService;

        public TodayResultQueryHandler(
            IUserSettingsService userSettingsService,
            ICurrencyLayerService currencyLayerService,
            IBinanceTickers binanceTickers,
            IPortofolioRepository portofolioRepository,
            IBinanceKlines binanceKlines)
        {
            _userSettingsService = userSettingsService;
            _currencyLayerService = currencyLayerService;
            _binanceTickers = binanceTickers;
            _portofolioRepository = portofolioRepository;
            _binanceKlines = binanceKlines;
        }

        public async Task<IOperationResult<TodayResultResponse>> Handle(TodayResultQuery request,
            CancellationToken cancellationToken)
        {
            var preferredCurrency = await _userSettingsService.GetPreferredCurrency(request.UserId);
            var miniTickers = _binanceTickers.GetCurrentMiniTickers();
            var portofolioCurrencies = await _portofolioRepository.RetrieveCurrenciesAsync(request.UserId);

            var currentCurrencyQuote = await _currencyLayerService.GetCurrentQuote(preferredCurrency);

            var resultItems = (await Task.WhenAll(portofolioCurrencies
                .Where(x => x.Symbol != CryptoniteConstants.BaseCryptoQuote)
                .Select(async portofolioCurrency =>
                {
                    var symbol = portofolioCurrency.Symbol + CryptoniteConstants.BaseCryptoQuote;
                    var ticker = miniTickers[symbol];
                    var closeQuote = await _binanceKlines.GetDayCloseQuote(symbol, DateTimeOffset.UtcNow.AddDays(-1));

                    var valueAtClose = closeQuote * portofolioCurrency.Amount;
                    var currentValue = ticker.LastPrice * portofolioCurrency.Amount;
                    return new ResultItem
                    {
                        Symbol = portofolioCurrency.Symbol,
                        PriceChangePercent = (currentValue / valueAtClose - 1) * 100,
                        Outcome = (currentValue - valueAtClose) * currentCurrencyQuote
                    };
                }))).OrderByDescending(x => x.Outcome).ToList();

            return ResultBuilder.Ok(new TodayResultResponse
            {
                Currency = preferredCurrency,
                Items = resultItems,
                TotalOutcome = resultItems.Select(x => x.Outcome).Sum()
            });
        }
    }
}