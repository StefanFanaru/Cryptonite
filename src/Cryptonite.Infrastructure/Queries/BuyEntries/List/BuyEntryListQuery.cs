using System;
using Cryptonite.Infrastructure.CQRS;
using Cryptonite.Infrastructure.Data.Common;

namespace Cryptonite.Infrastructure.Queries.BuyEntries.List
{
    public class BuyEntryListQuery : UserBasedSearchQuery<PageOf<BuyEntryListDto>>
    {
    }

    public class BuyEntryListDto
    {
        public string Id { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BoughtAmount { get; set; }
        public string PaymentCurrency { get; set; }
        public string BoughtCryptocurrency { get; set; }
        public DateTime BoughtAt { get; set; }
    }
}