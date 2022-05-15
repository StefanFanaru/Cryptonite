using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Cryptonite.Infrastructure.Services.Binance.Sockets.Dtos
{
    [ExcludeFromCodeCoverage]
    public class MiniTickerReceivedData
    {
        [JsonProperty("s")] public string Symbol { get; set; }
        [JsonProperty("c")] public decimal LastPrice { get; set; }
    }
}