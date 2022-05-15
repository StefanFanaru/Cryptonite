using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Core.Entities;
using Cryptonite.Infrastructure.CQRS.Operations;
using Cryptonite.Infrastructure.Data.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cryptonite.Infrastructure.Commands.TradeEntries.Delete;

public class DeleteTradeEntryCommandHandler : IRequestHandler<DeleteTradeEntryCommand, IOperationResult<Unit>>
{
    private readonly IPortofolioRepository _portofolioRepository;
    private readonly IRepository _repository;

    public DeleteTradeEntryCommandHandler(IRepository repository, IPortofolioRepository portofolioRepository)
    {
        _repository = repository;
        _portofolioRepository = portofolioRepository;
    }

    public async Task<IOperationResult<Unit>> Handle(DeleteTradeEntryCommand request, CancellationToken cancellationToken)
    {
        var query = _repository.Query<TradeEntry>()
            .Where(x => x.Id == request.Id && x.UserId == request.UserId);
        var entry = await query.FirstOrDefaultAsync(cancellationToken);

        if (entry == null)
        {
            ResultBuilder.NotFound();
        }

        await _repository.ExecuteTransactionalAsync(async transaction =>
        {
            await query.DeleteFromQueryAsync();
            await _portofolioRepository.DecreaseCryptocurrencyAmount(request.UserId, entry.GainedCryptocurrency, entry.GainedAmount);
            await _portofolioRepository.IncreaseCryptocurrencyAmount(request.UserId, entry.PaidCryptocurrency, entry.PaidAmount);
        });

        return ResultBuilder.NoContent();
    }
}