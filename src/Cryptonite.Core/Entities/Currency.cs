using System;
using System.Collections.Generic;
using Cryptonite.Core.Common;

namespace Cryptonite.Core.Entities
{
    public class Currency
    {
        public Currency(Dictionary<string, decimal> currencies)
        {
            Date = TimeProvider.UtcNow.Date;
            Currencies = currencies;
        }

        public Currency()
        {
        }

        public DateTime Date { get; set; }
        public Dictionary<string, decimal> Currencies { get; set; }
    }
}