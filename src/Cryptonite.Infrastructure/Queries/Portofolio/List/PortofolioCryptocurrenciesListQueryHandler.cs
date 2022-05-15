using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Core.Constants;
using Cryptonite.Core.Entities;
using Cryptonite.Infrastructure.Abstractions.Binance;
using Cryptonite.Infrastructure.Abstractions.Services.CurrencyLayer;
using Cryptonite.Infrastructure.Abstractions.UserSettingsService;
using Cryptonite.Infrastructure.CQRS.Operations;
using Cryptonite.Infrastructure.Data.Common;
using Cryptonite.Infrastructure.Data.Repositories;
using MediatR;

namespace Cryptonite.Infrastructure.Queries.Portofolio.List
{
    public class PortofolioCryptocurrenciesListQueryHandler : IRequestHandler<PortofolioCryptocurrenciesListQuery,
        IOperationResult<PageOf<PortofolioCryptocurrencyListDto>>>
    {
        private readonly ICurrencyLayerService _currencyLayerService;
        private readonly IRepository _repository;
        private readonly IBinanceTickers _tickers;
        private readonly IUserSettingsService _userSettingsService;

        public PortofolioCryptocurrenciesListQueryHandler(IRepository repository, IBinanceTickers tickers, IUserSettingsService userSettingsService,
            ICurrencyLayerService currencyLayerService)
        {
            _repository = repository;
            _tickers = tickers;
            _userSettingsService = userSettingsService;
            _currencyLayerService = currencyLayerService;
        }

        public async Task<IOperationResult<PageOf<PortofolioCryptocurrencyListDto>>> Handle(PortofolioCryptocurrenciesListQuery request,
            CancellationToken cancellationToken)
        {
            var query = _repository.Query<PortofolioCryptocurrency>().Where(x => x.Portofolio.UserId == request.UserId);
            var search = request.SearchParameters;
            var searchTerm = search.Term;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Symbol.ToString(CultureInfo.InvariantCulture).Contains(searchTerm,
                                             StringComparison.CurrentCultureIgnoreCase)
                                         || x.Amount.ToString(CultureInfo.InvariantCulture)
                                             .Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase));
            }

            var result = await query
                .Select(x => new PortofolioCryptocurrencyListDto
                {
                    Amount = x.Amount,
                    Symbol = x.Symbol,
                    InsertedAt = x.InsertedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .Order(search.SortColumn == null
                    ? nameof(PortofolioCryptocurrencyListDto.Amount)
                    : search.SortColumn ==
                      nameof(PortofolioCryptocurrencyListDto.Value)
                        ? nameof(PortofolioCryptocurrencyListDto.Amount)
                        : search.SortColumn, search.SortDirection)
                .GetPageAsync(search.PageIndex, search.PageSize, cancellationToken);

            var tickers = _tickers.GetCurrentCryptoCurrencyValues();
            var preferredCurrency = await _userSettingsService.GetPreferredCurrency(request.UserId);
            var currencyQuote = await _currencyLayerService.GetCurrentQuote(preferredCurrency);

            result.PageData.ForEach(x =>
            {
                var value = x.Symbol == CryptoniteConstants.BaseCryptoQuote ? currencyQuote * x.Amount : tickers[x.Symbol] * x.Amount * currencyQuote;
                x.Value = Math.Round(value, 2);
                x.ValueCurrency = preferredCurrency;
            });

            if (search.SortColumn is null or nameof(PortofolioCryptocurrencyListDto.Value))
            {
                result.PageData = search.SortDirection == SortDirection.Asc
                    ? result.PageData.OrderBy(x => x.Value).ToList()
                    : result.PageData.OrderByDescending(x => x.Value).ToList();
            }

            return ResultBuilder.Ok(result);
        }
    }
}