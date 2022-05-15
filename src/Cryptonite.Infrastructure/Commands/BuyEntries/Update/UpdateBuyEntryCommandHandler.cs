using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Core.Entities;
using Cryptonite.Infrastructure.CQRS.Operations;
using Cryptonite.Infrastructure.Data.Repositories;
using MediatR;

namespace Cryptonite.Infrastructure.Commands.BuyEntries.Update;

public class UpdateBuyEntryCommandHandler : IRequestHandler<UpdateBuyEntryCommand, IOperationResult<Unit>>
{
    private readonly IRepository _repository;

    public UpdateBuyEntryCommandHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IOperationResult<Unit>> Handle(UpdateBuyEntryCommand request, CancellationToken cancellationToken)
    {
        var affected = await _repository.Query<BuyEntry>().Where(x => x.Id == request.Id && x.UserId == request.UserId)
            .UpdateFromQueryAsync(x => new BuyEntry
            {
                BoughtCryptocurrency = request.BoughtCryptocurrency,
                PaymentCurrency = request.PaymentCurrency,
                BoughtAmount = request.BoughtAmount,
                PaidAmount = request.PaidAmount
            }, cancellationToken);

        return affected == 0 ? ResultBuilder.NotFound() : ResultBuilder.Ok();
    }
}