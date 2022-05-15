using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Infrastructure.Abstractions.Services.CurrencyLayer;
using Cryptonite.Infrastructure.CQRS.Operations;
using MediatR;

namespace Cryptonite.Infrastructure.Queries.Currencies.CurrenciesList
{
    public class CurrenciesListQueryHandler : IRequestHandler<CurrenciesListQuery, IOperationResult<List<string>>>
    {
        private readonly ICurrencyLayerService _currencyLayerService;

        public CurrenciesListQueryHandler(ICurrencyLayerService currencyLayerService)
        {
            _currencyLayerService = currencyLayerService;
        }

        public async Task<IOperationResult<List<string>>> Handle(CurrenciesListQuery request,
            CancellationToken cancellationToken)
        {
            var currencies = await _currencyLayerService.GetCurrentQuotes();
            return ResultBuilder.Ok(currencies.Keys.ToList());
        }
    }
}