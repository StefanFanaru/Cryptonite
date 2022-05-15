using Newtonsoft.Json;

namespace Cryptonite.Infrastructure.Services.Binance.Apis.Dtos
{
    public class TickerResponse
    {
        [JsonProperty("symbol")] public string Symbol { get; set; }
        [JsonProperty("lastPrice")] public decimal LastPrice { get; set; }
    }
}