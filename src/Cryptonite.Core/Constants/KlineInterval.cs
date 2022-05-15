namespace Cryptonite.Core.Constants
{
    public class KlineInterval
    {
        private KlineInterval(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public static KlineInterval Hour => new("1h");
        public static KlineInterval Day => new("1d");

        public static KlineInterval Week => new("1w");
        public static KlineInterval Month => new("1M");
    }
}