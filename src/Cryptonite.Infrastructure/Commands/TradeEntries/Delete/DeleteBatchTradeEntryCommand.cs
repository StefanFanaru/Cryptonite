using Cryptonite.Infrastructure.CQRS;
using MediatR;

namespace Cryptonite.Infrastructure.Commands.TradeEntries.Delete;

public class DeleteBatchTradeEntryCommand : UserBasedCommand<Unit>
{
    public string[] Ids { get; set; }
}