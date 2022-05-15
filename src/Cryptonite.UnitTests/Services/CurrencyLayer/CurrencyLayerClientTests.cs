using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Cryptonite.Infrastructure.Common;
using Cryptonite.Infrastructure.Helpers;
using Cryptonite.Infrastructure.Services.CurrencyLayer;
using Cryptonite.UnitTests.Helpers;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Xunit;

namespace Cryptonite.UnitTests.Services.CurrencyLayer
{
    public class CurrencyLayerClientTests
    {
        private readonly Dictionary<string, decimal> _parsedCurrencies =
            Placeholders.ParsetCurrencies.FromJson<Dictionary<string, decimal>>();

        private readonly Dictionary<string, decimal> _returnedCurrencies =
            Placeholders.CurrencyLayerResponse.FromJson<CurrencyLayerClient.Response>().Quotes;

        private CurrencyLayerClient CreateSut()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(new CurrencyLayerClient.Response
                {
                    Quotes = _returnedCurrencies
                }.ToJson())
            };

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
            var httpClient = new HttpClient(handlerMock.Object);

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>()))
                .Returns(httpClient).Verifiable();

            return new CurrencyLayerClient(ServiceHelpers.CreateConfiguration(), httpClientFactoryMock.Object);
        }

        [Fact]
        private async Task Fetches_currencies_quotes_and_parses_response()
        {
            var sut = CreateSut();
            var actual = await sut.RequestCurrenciesQuotes();
            actual.Should().BeEquivalentTo(_parsedCurrencies);
        }

        [Fact]
        private async Task Fetches_historical_currencies_quotes_and_parses_response()
        {
            var sut = CreateSut();
            var actual = await sut.RequestHistoricalCurrenciesQuotes(new DateTime(2021, 09, 25));
            actual.Should().BeEquivalentTo(_parsedCurrencies);
        }
    }
}