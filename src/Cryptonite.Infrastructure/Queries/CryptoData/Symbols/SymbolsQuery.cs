using System.Collections.Generic;
using Cryptonite.Infrastructure.CQRS.Operations;
using MediatR;

namespace Cryptonite.Infrastructure.Queries.CryptoData.Symbols
{
    public class SymbolsQuery : IRequest<IOperationResult<List<string>>>
    {
    }
}