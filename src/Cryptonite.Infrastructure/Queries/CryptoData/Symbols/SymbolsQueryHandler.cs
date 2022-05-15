using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Core.Constants;
using Cryptonite.Infrastructure.Abstractions.Binance;
using Cryptonite.Infrastructure.CQRS.Operations;
using MediatR;

namespace Cryptonite.Infrastructure.Queries.CryptoData.Symbols
{
    public class SymbolsQueryHandler : IRequestHandler<SymbolsQuery, IOperationResult<List<string>>>
    {
        private readonly IBinanceTickers _binanceTickers;

        public SymbolsQueryHandler(IBinanceTickers binanceTickers)
        {
            _binanceTickers = binanceTickers;
        }

        public Task<IOperationResult<List<string>>> Handle(SymbolsQuery request,
            CancellationToken cancellationToken)
        {
            var symbols = _binanceTickers.GetCurrentMiniTickers().Keys
                .Select(x => x.Replace(CryptoniteConstants.BaseCryptoQuote, "")).Append(CryptoniteConstants.BaseCryptoQuote).ToList();

            return Task.FromResult(ResultBuilder.Ok(symbols));
        }
    }
}