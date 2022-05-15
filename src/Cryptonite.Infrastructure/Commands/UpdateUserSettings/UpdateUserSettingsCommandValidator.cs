using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace Cryptonite.Infrastructure.Commands.UpdateUserSettings
{
    [ExcludeFromCodeCoverage]
    public class UpdateUserSettingsCommandValidator : AbstractValidator<UpdateUserSettingsCommand>
    {
        public UpdateUserSettingsCommandValidator()
        {
            RuleFor(x => x.UserId).Length(36);
            RuleFor(x => x.PreferredCurrency).Length(3);
            RuleFor(x => x.BankAccountCurrency).Length(3);
            RuleFor(x => x.BankConversionMargin).LessThan(100).GreaterThanOrEqualTo(0);
        }
    }
}