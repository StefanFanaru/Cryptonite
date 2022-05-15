using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Core.Entities;
using Cryptonite.Infrastructure.CQRS.Operations;
using Cryptonite.Infrastructure.Data.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cryptonite.Infrastructure.Commands.TradeEntries.Delete;

public class DeleteBatchTradeEntryCommandHandler : IRequestHandler<DeleteBatchTradeEntryCommand, IOperationResult<Unit>>
{
    private readonly IPortofolioRepository _portofolioRepository;
    private readonly IRepository _repository;

    public DeleteBatchTradeEntryCommandHandler(IRepository repository, IPortofolioRepository portofolioRepository)
    {
        _repository = repository;
        _portofolioRepository = portofolioRepository;
    }

    public async Task<IOperationResult<Unit>> Handle(DeleteBatchTradeEntryCommand request, CancellationToken cancellationToken)
    {
        var query = _repository.Query<TradeEntry>()
            .Where(x => request.Ids.Contains(x.Id) && x.UserId == request.UserId);
        var entries = await query.ToListAsync(cancellationToken);

        if (entries.Count == 0)
        {
            ResultBuilder.NotFound();
        }

        await _repository.ExecuteTransactionalAsync(async transaction =>
        {
            await query.DeleteFromQueryAsync();
            foreach (var entry in entries)
            {
                await _portofolioRepository.DecreaseCryptocurrencyAmount(request.UserId, entry.GainedCryptocurrency, entry.GainedAmount);
                await _portofolioRepository.IncreaseCryptocurrencyAmount(request.UserId, entry.PaidCryptocurrency, entry.PaidAmount);
            }
        });

        return ResultBuilder.NoContent();
    }
}