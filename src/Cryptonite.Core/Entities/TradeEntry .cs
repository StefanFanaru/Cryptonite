using System;

namespace Cryptonite.Core.Entities
{
    public class TradeEntry : Entity<string>
    {
        public TradeEntry() : base(Guid.NewGuid().ToString())
        {
        }

        public string UserId { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal GainedAmount { get; set; }
        public string PaidCryptocurrency { get; set; }
        public string GainedCryptocurrency { get; set; }
        public DateTime TradedAt { get; set; }
    }
}