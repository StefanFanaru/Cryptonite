using System.Diagnostics.CodeAnalysis;
using Cryptonite.Infrastructure.CQRS;

namespace Cryptonite.Infrastructure.Queries.Portofolio.Results.AllTime
{
    [ExcludeFromCodeCoverage]
    public class AllTimeResultQuery : UserBasedQuery<AllTimeResultQueryResult>
    {
    }

    [ExcludeFromCodeCoverage]
    public class AllTimeResultQueryResult
    {
        public string Currency { get; set; }
        public decimal Result { get; set; }
    }
}