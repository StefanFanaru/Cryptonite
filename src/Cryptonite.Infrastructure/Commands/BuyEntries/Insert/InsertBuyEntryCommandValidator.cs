using System;
using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace Cryptonite.Infrastructure.Commands.BuyEntries.Insert
{
    [ExcludeFromCodeCoverage]
    public class InsertBuyEntryValidator : AbstractValidator<InsertBuyEntryCommand>
    {
        public InsertBuyEntryValidator()
        {
            RuleFor(x => x.UserId).Length(36);
            RuleFor(x => x.PaidAmount).GreaterThan(0);
            RuleFor(x => x.BoughtAmount).GreaterThan(0);
            RuleFor(x => x.PaymentCurrency).Length(3);
            RuleFor(x => x.BankAccountCurrency).Length(3);
            RuleFor(x => x.BoughtCryptocurrency).NotEmpty();
            RuleFor(x => x.BoughtAt).LessThanOrEqualTo(DateTime.UtcNow);
            RuleFor(x => x.BankConversionMargin).LessThan(100).GreaterThanOrEqualTo(0);
        }
    }
}