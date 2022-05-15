using Cryptonite.Infrastructure.CQRS;
using MediatR;

namespace Cryptonite.Infrastructure.Commands.TradeEntries.Update;

public class UpdateTradeEntryCommand : UserBasedIdCommand<Unit>
{
    public decimal PaidAmount { get; set; }
    public decimal GainedAmount { get; set; }
    public string PaidCryptocurrency { get; set; }
    public string GainedCryptocurrency { get; set; }
}