using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Core.Entities;
using Cryptonite.Infrastructure.Abstractions.Services.CurrencyLayer;
using Cryptonite.Infrastructure.CQRS.Operations;
using Cryptonite.Infrastructure.Data.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cryptonite.Infrastructure.Commands.BuyEntries.Insert
{
    public class InsertBuyEntryCommandHandler : IRequestHandler<InsertBuyEntryCommand, IOperationResult<Unit>>
    {
        private readonly ICurrencyLayerService _currencyLayerService;
        private readonly IPortofolioRepository _portofolioRepository;
        private readonly IRepository _repository;

        public InsertBuyEntryCommandHandler(
            IRepository repository,
            ICurrencyLayerService currencyLayerService,
            IPortofolioRepository portofolioRepository)
        {
            _repository = repository;
            _currencyLayerService = currencyLayerService;
            _portofolioRepository = portofolioRepository;
        }

        public async Task<IOperationResult<Unit>> Handle(InsertBuyEntryCommand request, CancellationToken cancellationToken)
        {
            var paidUsd = await CalculatePaidUsdAmount(request);

            var entry = new BuyEntry
            {
                UserId = request.UserId,
                PaymentCurrency = request.PaymentCurrency,
                BoughtAt = request.BoughtAt,
                BoughtCryptocurrency = request.BoughtCryptocurrency,
                BoughtAmount = request.BoughtAmount,
                PaidAmount = request.PaidAmount,
                PaidUsd = paidUsd
            };

            var exists = await _repository.Query<BuyEntry>()
                .Where(x => x.BoughtAt.Date == request.BoughtAt.Date && x.UserId == request.UserId)
                .Where(x => x.BoughtCryptocurrency == request.BoughtCryptocurrency)
                .Where(x => x.PaymentCurrency == request.PaymentCurrency)
                .Where(x => x.BoughtAmount == request.BoughtAmount)
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

                await _portofolioRepository.IncreaseCryptocurrencyAmount(request.UserId, request.BoughtCryptocurrency,
                    request.BoughtAmount, request.BoughtAt);
            });

            return ResultBuilder.Ok();
        }

        private async Task<decimal> CalculatePaidUsdAmount(InsertBuyEntryCommand command)
        {
            if (command.BankAccountCurrency == command.PaymentCurrency || command.BankConversionMargin == 0)
            {
                if (command.PaymentCurrency == "USD")
                {
                    return command.PaidAmount;
                }

                var usdQuote = await _currencyLayerService.GetHistoricalQuote(command.PaymentCurrency, command.BoughtAt);
                return 1 / usdQuote * command.PaidAmount;
            }

            var usdPaidCurrencyQuote = await _currencyLayerService.GetHistoricalQuote(command.PaymentCurrency, command.BoughtAt);
            return 1 / usdPaidCurrencyQuote * (command.BankConversionMargin / 100 + 1) * command.PaidAmount;
        }
    }
}