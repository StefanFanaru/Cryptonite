using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Core.Entities;
using Cryptonite.Infrastructure.CQRS.Operations;
using Cryptonite.Infrastructure.Data.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cryptonite.Infrastructure.Commands.BuyEntries.Delete;

public class DeleteBuyEntryCommandHandler : IRequestHandler<DeleteBuyEntryCommand, IOperationResult<Unit>>
{
    private readonly IPortofolioRepository _portofolioRepository;
    private readonly IRepository _repository;

    public DeleteBuyEntryCommandHandler(IRepository repository, IPortofolioRepository portofolioRepository)
    {
        _repository = repository;
        _portofolioRepository = portofolioRepository;
    }

    public async Task<IOperationResult<Unit>> Handle(DeleteBuyEntryCommand request, CancellationToken cancellationToken)
    {
        var query = _repository.Query<BuyEntry>()
            .Where(x => x.Id == request.Id && x.UserId == request.UserId);
        var entry = await query.FirstOrDefaultAsync(cancellationToken);

        if (entry == null)
        {
            ResultBuilder.NotFound();
        }

        await _repository.ExecuteTransactionalAsync(async transaction =>
        {
            await query.DeleteFromQueryAsync(cancellationToken);
            await _portofolioRepository.DecreaseCryptocurrencyAmount(request.UserId, entry.BoughtCryptocurrency, entry.BoughtAmount);
        });

        return ResultBuilder.NoContent();
    }
}