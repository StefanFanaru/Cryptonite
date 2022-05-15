using System;

namespace Cryptonite.Infrastructure.Commands.ImportEntries;

public class BuyEntryImportDto
{
    public DateTime BoughtAt { get; set; }
    public decimal PaidAmount { get; set; }
    public string PaymentCurrency { get; set; }
    public decimal BoughtCryptoRate { get; set; }
    public decimal Fees { get; set; }
    public decimal BoughtAmount { get; set; }
    public string BoughtCryptocurrency { get; set; }
}