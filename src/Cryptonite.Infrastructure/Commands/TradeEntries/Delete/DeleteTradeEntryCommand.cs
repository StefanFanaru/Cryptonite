using Cryptonite.Infrastructure.CQRS;
using MediatR;

namespace Cryptonite.Infrastructure.Commands.TradeEntries.Delete;

public class DeleteTradeEntryCommand : UserBasedIdCommand<Unit>
{
}