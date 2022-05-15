using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Core.Entities;
using Cryptonite.Infrastructure.CQRS.Operations;
using Cryptonite.Infrastructure.Data.Common;
using Cryptonite.Infrastructure.Data.Repositories;
using MediatR;

namespace Cryptonite.Infrastructure.Queries.BuyEntries.List
{
    public class BuyEntryListQueryHandler : IRequestHandler<BuyEntryListQuery,
        IOperationResult<PageOf<BuyEntryListDto>>>
    {
        private readonly IRepository _repository;

        public BuyEntryListQueryHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IOperationResult<PageOf<BuyEntryListDto>>> Handle(BuyEntryListQuery request,
            CancellationToken cancellationToken)
        {
            var query = _repository.Query<BuyEntry>().Where(x => x.UserId == request.UserId);
            var search = request.SearchParameters;
            var searchTerm = search.Term;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.BoughtCryptocurrency.ToString(CultureInfo.InvariantCulture).Contains(searchTerm,
                                             StringComparison.CurrentCultureIgnoreCase)
                                         || x.PaymentCurrency.ToString(CultureInfo.InvariantCulture)
                                             .Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                                         || x.Id.Contains(searchTerm));
            }

            return ResultBuilder.Ok(await query
                .Select(x => new BuyEntryListDto
                {
                    Id = x.Id,
                    BoughtCryptocurrency = x.BoughtCryptocurrency,
                    PaymentCurrency = x.PaymentCurrency,
                    BoughtAmount = x.BoughtAmount,
                    PaidAmount = x.PaidAmount,
                    BoughtAt = x.BoughtAt
                })
                .Order(search.SortColumn ?? nameof(BuyEntry.BoughtAt), search.SortDirection)
                .GetPageAsync(search.PageIndex, search.PageSize, cancellationToken));
        }
    }
}