using System;

namespace Cryptonite.Core.Entities
{
    public class BuyEntry : Entity<string>
    {
        public BuyEntry() : base(Guid.NewGuid().ToString())
        {
        }

        public string UserId { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BoughtAmount { get; set; }
        public string PaymentCurrency { get; set; }
        public string BoughtCryptocurrency { get; set; }
        public DateTime BoughtAt { get; set; }
        public decimal PaidUsd { get; set; }
    }
}