using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Core.Entities;
using Cryptonite.Infrastructure.CQRS.Operations;
using Cryptonite.Infrastructure.Data.Common;
using Cryptonite.Infrastructure.Data.Repositories;
using MediatR;

namespace Cryptonite.Infrastructure.Queries.TradeEntries.List
{
    public class TradeEntryListQueryHandler : IRequestHandler<TradeEntryListQuery,
        IOperationResult<PageOf<TradeEntryListDto>>>
    {
        private readonly IRepository _repository;

        public TradeEntryListQueryHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IOperationResult<PageOf<TradeEntryListDto>>> Handle(TradeEntryListQuery request,
            CancellationToken cancellationToken)
        {
            var query = _repository.Query<TradeEntry>().Where(x => x.UserId == request.UserId);
            var search = request.SearchParameters;
            var searchTerm = search.Term;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.GainedCryptocurrency.ToString(CultureInfo.InvariantCulture).Contains(searchTerm,
                                             StringComparison.CurrentCultureIgnoreCase)
                                         || x.PaidCryptocurrency.ToString(CultureInfo.InvariantCulture)
                                             .Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                                         || x.Id.Contains(searchTerm));
            }

            return ResultBuilder.Ok(await query
                .Select(x => new TradeEntryListDto
                {
                    Id = x.Id,
                    GainedCryptocurrency = x.GainedCryptocurrency,
                    PaidCryptocurrency = x.PaidCryptocurrency,
                    GainedAmount = x.GainedAmount,
                    PaidAmount = x.PaidAmount,
                    TradedAt = x.TradedAt
                })
                .Order(search.SortColumn ?? nameof(TradeEntry.TradedAt), search.SortDirection)
                .GetPageAsync(search.PageIndex, search.PageSize, cancellationToken));
        }
    }
}