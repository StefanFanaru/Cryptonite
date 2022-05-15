using System;

namespace Cryptonite.Core.Entities
{
    public class PortofolioCryptocurrency
    {
        public string Symbol { get; set; }
        public decimal Amount { get; set; }
        public DateTime InsertedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Portofolio Portofolio { get; set; }
        public string PortofolioId { get; set; }
    }
}