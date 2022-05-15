using System;
using Cryptonite.Infrastructure.CQRS;
using MediatR;

namespace Cryptonite.Infrastructure.Commands.BuyEntries.Insert
{
    public class InsertBuyEntryCommand : UserBasedCommand<Unit>
    {
        public decimal PaidAmount { get; set; }
        public decimal BoughtAmount { get; set; }
        public decimal BankConversionMargin { get; set; }
        public string PaymentCurrency { get; set; }
        public string BoughtCryptocurrency { get; set; }
        public string BankAccountCurrency { get; set; }
        public DateTime BoughtAt { get; set; }
    }
}