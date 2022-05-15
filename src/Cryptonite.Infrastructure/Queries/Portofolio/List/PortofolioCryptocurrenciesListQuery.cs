using System;
using Cryptonite.Infrastructure.CQRS;
using Cryptonite.Infrastructure.Data.Common;

namespace Cryptonite.Infrastructure.Queries.Portofolio.List
{
    public class PortofolioCryptocurrenciesListQuery : UserBasedSearchQuery<PageOf<PortofolioCryptocurrencyListDto>>
    {
    }

    public class PortofolioCryptocurrencyListDto
    {
        public string Symbol { get; set; }
        public decimal Amount { get; set; }
        public decimal Value { get; set; }
        public string ValueCurrency { get; set; }
        public DateTime InsertedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}