using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Cryptonite.Infrastructure.Abstractions.Services.CurrencyLayer;
using Cryptonite.Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;

namespace Cryptonite.Infrastructure.Services.CurrencyLayer
{
    public class CurrencyLayerClient : ICurrencyLayerClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public CurrencyLayerClient(IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
        }

        public async Task<Dictionary<string, decimal>> RequestCurrenciesQuotes()
        {
            var response = await _clientFactory.CreateClient().GetAsync<Response>(
                $"http://api.currencylayer.com/live?access_key={_configuration["Secrets:CurrencyLayerAPIKey"]}&source=USD");

            if (response.Quotes?.Count == 0)
            {
                throw new Exception("CurrencyLayer API did not returned the quotes");
            }

            return ParseResponse(response.Quotes);
        }

        public async Task<Dictionary<string, decimal>> RequestHistoricalCurrenciesQuotes(DateTime date)
        {
            var response = await _clientFactory.CreateClient().GetAsync<Response>(
                $"http://api.currencylayer.com/historical?access_key={_configuration["Secrets:CurrencyLayerAPIKey"]}&source=USD&date={date:yyyy-MM-dd}");

            if (response.Quotes?.Count == 0)
            {
                throw new Exception($"CurrencyLayer API found no historical data for {date:yyyy-MM-dd}");
            }

            return ParseResponse(response.Quotes);
        }

        private Dictionary<string, decimal> ParseResponse(Dictionary<string, decimal> response)
        {
            const string prefix = "USD";
            var result = new Dictionary<string, decimal>();
            foreach (var (key, value) in response)
            {
                result.Add(key.Remove(0, prefix.Length), value);
            }

            return result;
        }

        public class Response
        {
            public Dictionary<string, decimal> Quotes { get; set; }
        }
    }
}