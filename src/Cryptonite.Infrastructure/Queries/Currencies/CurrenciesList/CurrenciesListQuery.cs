using System.Collections.Generic;
using Cryptonite.Infrastructure.CQRS.Operations;
using MediatR;

namespace Cryptonite.Infrastructure.Queries.Currencies.CurrenciesList
{
    public class CurrenciesListQuery : IRequest<IOperationResult<List<string>>>
    {
    }
}