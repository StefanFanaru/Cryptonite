using Cryptonite.Infrastructure.CQRS;
using MediatR;

namespace Cryptonite.Infrastructure.Commands.BuyEntries.Update;

public class UpdateBuyEntryCommand : UserBasedIdCommand<Unit>
{
    public decimal PaidAmount { get; set; }
    public decimal BoughtAmount { get; set; }
    public string PaymentCurrency { get; set; }
    public string BoughtCryptocurrency { get; set; }
}