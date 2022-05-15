using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Core.Entities;
using Cryptonite.Infrastructure.CQRS.Operations;
using Cryptonite.Infrastructure.Data.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cryptonite.Infrastructure.Commands.TradeEntries.Insert
{
    public class InsertTradeEntryCommandHandler : IRequestHandler<InsertTradeEntryCommand, IOperationResult<Unit>>
    {
        private readonly IPortofolioRepository _portofolioRepository;
        private readonly IRepository _repository;

        public InsertTradeEntryCommandHandler(IRepository repository, IPortofolioRepository portofolioRepository)
        {
            _repository = repository;
            _portofolioRepository = portofolioRepository;
        }

        public async Task<IOperationResult<Unit>> Handle(InsertTradeEntryCommand request, CancellationToken cancellationToken)
        {
            var entry = new TradeEntry
            {
                UserId = request.UserId,
                PaidCryptocurrency = request.PaidCryptocurrency,
                TradedAt = request.TradedAt,
                GainedCryptocurrency = request.GainedCryptocurrency,
                GainedAmount = request.GainedAmount,
                PaidAmount = request.PaidAmount
            };

            var exists = await _repository.Query<TradeEntry>()
                .Where(x => x.TradedAt.Date == request.TradedAt.Date && x.UserId == request.UserId)
                .Where(x => x.GainedCryptocurrency == request.GainedCryptocurrency)
                .Where(x => x.GainedAmount == request.GainedAmount)
                .Where(x => x.PaidCryptocurrency == request.PaidCryptocurrency)
                .Where(x => x.PaidAmount == request.PaidAmount)
                .AnyAsync(cancellationToken);

            if (exists)
            {
                return ResultBuilder.Error<Unit>(HttpStatusCode.BadRequest, "Entry already exists").Build();
            }

            await _repository.ExecuteTransactionalAsync(async transaction =>
            {
                await _repository.InsertAsync(entry);
                await _repository.SaveAsync();

                await _portofolioRepository.IncreaseCryptocurrencyAmount(request.UserId, request.GainedCryptocurrency,
                    request.GainedAmount, request.TradedAt);

                await _portofolioRepository.DecreaseCryptocurrencyAmount(request.UserId, request.PaidCryptocurrency,
                    request.PaidAmount, request.TradedAt);

                await _repository.SaveAsync();
            });

            return ResultBuilder.Ok();
        }
    }
}