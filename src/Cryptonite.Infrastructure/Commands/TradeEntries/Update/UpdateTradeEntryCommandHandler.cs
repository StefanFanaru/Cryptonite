using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Core.Entities;
using Cryptonite.Infrastructure.CQRS.Operations;
using Cryptonite.Infrastructure.Data.Repositories;
using MediatR;

namespace Cryptonite.Infrastructure.Commands.TradeEntries.Update;

public class UpdateTradeEntryCommandHandler : IRequestHandler<UpdateTradeEntryCommand, IOperationResult<Unit>>
{
    private readonly IRepository _repository;

    public UpdateTradeEntryCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IOperationResult<Unit>> Handle(UpdateTradeEntryCommand request, CancellationToken cancellationToken)
    {
        var affected = await _repository.Query<TradeEntry>().Where(x => x.Id == request.Id && x.UserId == request.UserId)
            .UpdateFromQueryAsync(x => new TradeEntry
            {
                GainedAmount = request.GainedAmount,
                PaidCryptocurrency = request.PaidCryptocurrency,
                GainedCryptocurrency = request.GainedCryptocurrency,
                PaidAmount = request.PaidAmount
            }, cancellationToken);

        return affected == 0 ? ResultBuilder.NotFound() : ResultBuilder.Ok();
    }
}