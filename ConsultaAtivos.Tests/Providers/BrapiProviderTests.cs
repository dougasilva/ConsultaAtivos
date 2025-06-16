using ConsultaAtivos.Domain.Configuration;
using ConsultaAtivos.Domain.Entities;
using ConsultaAtivos.Infra.Providers;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;
using System.Text.Json;

namespace ConsultaAtivos.Tests.Providers
{
    public class BrapiProviderTests
    {
        [Fact]
        public async Task ObterCandlesAsync_DeveRetornarCandleFiltradoPorPeriodo()
        {
            // Arrange
            var jsonResponse = await File.ReadAllTextAsync("Mocks/Brapi_HistoricalQuote.json");

            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When("https://brapi.dev/api/quote/*")
                .Respond("application/json", jsonResponse);

            var httpClient = mockHttp.ToHttpClient();

            var settings = new BrapiSettings
            {
                ApiKey = "FAKE_API_KEY"
            };

            var options = Options.Create(settings);
            var provider = new BrapiProvider(httpClient, options);

            var inicio = new DateTime(2025, 06, 11);
            var fim = new DateTime(2025, 06, 12);

            // Act
            var result = await provider.ObterCandlesAsync("PETR4", inicio, fim);

            // Assert
            Assert.NotEmpty(result);
            Assert.All(result, quote =>
            {
                Assert.InRange(quote.Data, inicio, fim);
            });
        }
    }
}
