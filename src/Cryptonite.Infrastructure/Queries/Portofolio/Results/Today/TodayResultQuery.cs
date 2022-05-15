using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Cryptonite.Infrastructure.CQRS;

namespace Cryptonite.Infrastructure.Queries.Portofolio.Results.Today
{
    [ExcludeFromCodeCoverage]
    public class TodayResultQuery : UserBasedQuery<TodayResultResponse>
    {
    }

    [ExcludeFromCodeCoverage]
    public class TodayResultResponse
    {
        public string Currency { get; set; }
        public decimal TotalOutcome { get; set; }
        public List<ResultItem> Items { get; set; }
    }

    public class ResultItem
    {
        public string Symbol { get; set; }
        public decimal PriceChangePercent { get; set; }
        public decimal Outcome { get; set; }
    }
}