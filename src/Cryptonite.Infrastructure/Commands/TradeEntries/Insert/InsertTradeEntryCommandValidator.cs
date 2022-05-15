using System;
using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace Cryptonite.Infrastructure.Commands.TradeEntries.Insert
{
    [ExcludeFromCodeCoverage]
    public class InsertTradeEntryValidator : AbstractValidator<InsertTradeEntryCommand>
    {
        public InsertTradeEntryValidator()
        {
            RuleFor(x => x.UserId).Length(36);
            RuleFor(x => x.PaidAmount).GreaterThan(0);
            RuleFor(x => x.GainedAmount).GreaterThan(0);
            RuleFor(x => x.PaidCryptocurrency).NotEmpty().NotEqual(x => x.GainedCryptocurrency);
            RuleFor(x => x.GainedCryptocurrency).NotEmpty();
            RuleFor(x => x.TradedAt).LessThanOrEqualTo(DateTime.UtcNow);
        }
    }
}