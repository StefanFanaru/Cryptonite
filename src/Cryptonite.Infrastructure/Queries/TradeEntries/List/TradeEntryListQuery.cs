using System;
using Cryptonite.Infrastructure.CQRS;
using Cryptonite.Infrastructure.Data.Common;

namespace Cryptonite.Infrastructure.Queries.TradeEntries.List
{
    public class TradeEntryListQuery : UserBasedSearchQuery<PageOf<TradeEntryListDto>>
    {
    }

    public class TradeEntryListDto
    {
        public string Id { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal GainedAmount { get; set; }
        public string PaidCryptocurrency { get; set; }
        public string GainedCryptocurrency { get; set; }
        public DateTime TradedAt { get; set; }
    }
}