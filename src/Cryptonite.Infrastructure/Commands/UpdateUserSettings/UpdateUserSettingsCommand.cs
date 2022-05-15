using Cryptonite.Infrastructure.CQRS;
using MediatR;

namespace Cryptonite.Infrastructure.Commands.UpdateUserSettings
{
    public class UpdateUserSettingsCommand : UserBasedCommand<Unit>
    {
        public string PreferredCurrency { get; set; }
        public string BankAccountCurrency { get; set; }
        public decimal BankConversionMargin { get; set; }
    }
}