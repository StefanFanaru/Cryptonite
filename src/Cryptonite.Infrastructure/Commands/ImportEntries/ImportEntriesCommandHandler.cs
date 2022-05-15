using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Core.Enums;
using Cryptonite.Infrastructure.Abstractions.UserSettingsService;
using Cryptonite.Infrastructure.Commands.BuyEntries.Insert;
using Cryptonite.Infrastructure.Commands.TradeEntries.Insert;
using Cryptonite.Infrastructure.CQRS.Operations;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Cryptonite.Infrastructure.Commands.ImportEntries
{
    public class ImportEntriesCommandHandler : IRequestHandler<ImportEntriesCommand, IOperationResult<Unit>>
    {
        private readonly IMediator _mediator;
        private readonly IUserSettingsService _userSettingsService;

        public ImportEntriesCommandHandler(
            IUserSettingsService userSettingsService,
            IMediator mediator)
        {
            _userSettingsService = userSettingsService;
            _mediator = mediator;
        }

        public async Task<IOperationResult<Unit>> Handle(ImportEntriesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                switch (request.Type)
                {
                    case ImportType.Buys:
                        await HandleBuysFile(request.File, request.UserId, cancellationToken);
                        break;
                    case ImportType.Trades:
                        await HandleTradesFile(request.File, request.UserId, cancellationToken);
                        break;
                    case ImportType.Sells:
                        throw new Exception("Sells import is not implemented yet");
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Error while importing entries");
                return ResultBuilder.Error<Unit>(HttpStatusCode.BadRequest, "Error while importing entries").Build();
            }

            return ResultBuilder.Ok();
        }

        private async Task HandleBuysFile(IFormFile file, string userId, CancellationToken cancellationToken)
        {
            var ms = new MemoryStream();
            await file.CopyToAsync(ms, cancellationToken);
            var buyEntries = ImportEntriesHelpers.ReadyBuyEntries(ms);
            var userSettings = await _userSettingsService.GetUserSettings(userId);

            foreach (var buyEntryImportDto in buyEntries)
            {
                var command = new InsertBuyEntryCommand
                {
                    UserId = userId,
                    PaymentCurrency = buyEntryImportDto.PaymentCurrency,
                    BoughtAt = buyEntryImportDto.BoughtAt,
                    BoughtCryptocurrency = buyEntryImportDto.BoughtCryptocurrency,
                    BoughtAmount = buyEntryImportDto.BoughtAmount,
                    PaidAmount = buyEntryImportDto.PaidAmount,
                    BankAccountCurrency = userSettings.BankAccountCurrency,
                    BankConversionMargin = userSettings.BankConversionMargin
                };
                await _mediator.Send(command, cancellationToken);
            }
        }

        private async Task HandleTradesFile(IFormFile file, string userId, CancellationToken cancellationToken)
        {
            var ms = new MemoryStream();
            await file.CopyToAsync(ms, cancellationToken);
            var tradeEntries = ImportEntriesHelpers.ReadTradeEntries(ms);

            foreach (var buyEntryImportDto in tradeEntries)
            {
                var command = new InsertTradeEntryCommand
                {
                    UserId = userId,
                    PaidAmount = buyEntryImportDto.PaidAmount,
                    GainedAmount = buyEntryImportDto.GainedAmount,
                    GainedCryptocurrency = buyEntryImportDto.GainedCryptocurrencySymbol,
                    PaidCryptocurrency = buyEntryImportDto.PaidCryptocurrencySymbol,
                    TradedAt = buyEntryImportDto.TradedAt
                };
                await _mediator.Send(command, cancellationToken);
            }
        }
    }
}