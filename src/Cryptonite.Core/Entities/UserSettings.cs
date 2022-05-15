namespace Cryptonite.Core.Entities
{
    public class UserSettings
    {
        public string UserId { get; set; }
        public string PreferredCurrency { get; set; }
        public string BankAccountCurrency { get; set; }
        public decimal BankConversionMargin { get; set; }
    }
}