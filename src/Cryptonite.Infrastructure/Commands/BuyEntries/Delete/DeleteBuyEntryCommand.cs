using Cryptonite.Infrastructure.CQRS;
using MediatR;

namespace Cryptonite.Infrastructure.Commands.BuyEntries.Delete;

public class DeleteBuyEntryCommand : UserBasedIdCommand<Unit>
{
}