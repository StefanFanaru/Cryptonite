using System;

namespace Cryptonite.Infrastructure.Commands.ImportEntries;

public class TradeEntryImportDto
{
    public DateTime TradedAt { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal GainedAmount { get; set; }
    public decimal Fee { get; set; }
    public string PaidCryptocurrencySymbol { get; set; }
    public string GainedCryptocurrencySymbol { get; set; }
    public decimal GainedCryptocurrencyRate { get; set; }
}