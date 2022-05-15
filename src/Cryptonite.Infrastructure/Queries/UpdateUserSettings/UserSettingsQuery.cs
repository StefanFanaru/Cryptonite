using Cryptonite.Infrastructure.CQRS;

namespace Cryptonite.Infrastructure.Queries.UpdateUserSettings
{
    public class UserSettingsQuery : UserBasedQuery<UserSettingsQueryResponse>
    {
    }

    public class UserSettingsQueryResponse
    {
        public string PreferredCurrency { get; set; }
        public string BankAccountCurrency { get; set; }
        public decimal BankConversionMargin { get; set; }
    }
}