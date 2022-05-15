using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Cryptonite.Infrastructure.CQRS;
using Cryptonite.Infrastructure.Services.Portofolio.Dtos;

namespace Cryptonite.Infrastructure.Queries.Portofolio.Distribution
{
    [ExcludeFromCodeCoverage]
    public class DistributionQuery : UserBasedQuery<DistributionQueryResult>
    {
    }

    [ExcludeFromCodeCoverage]
    public class DistributionQueryResult
    {
        public decimal TotalValue { get; set; }
        public string Currency { get; set; }
        public IEnumerable<DistributionItem> Items { get; set; }
    }
}