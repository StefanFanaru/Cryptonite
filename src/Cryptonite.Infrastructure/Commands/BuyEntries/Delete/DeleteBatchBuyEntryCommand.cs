using Cryptonite.Infrastructure.CQRS;
using MediatR;

namespace Cryptonite.Infrastructure.Commands.BuyEntries.Delete;

public class DeleteBatchBuyEntryCommand : UserBasedCommand<Unit>
{
    public string[] Ids { get; set; }
}