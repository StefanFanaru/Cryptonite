using System;
using System.Collections.Generic;

namespace Cryptonite.Core.Entities
{
    public class Portofolio : Entity<string>
    {
        public Portofolio(string userId) : base(Guid.NewGuid().ToString())
        {
            UserId = userId;
        }

        public Portofolio()
        {
        }

        public string UserId { get; set; }
        public List<PortofolioCryptocurrency> Cryptocurrencies { get; set; }
        public int Transactions { get; set; }
        public DateTime LastTransactionAt { get; set; }
    }
}