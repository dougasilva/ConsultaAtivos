using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using ConsultaAtivos.Infra.Providers;
using ConsultaAtivos.Domain.Configuration;
using ConsultaAtivos.Domain.Entities;
using RichardSzalay.MockHttp;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Collections.Generic;

namespace ConsultaAtivos.Tests.Providers
{
    public class AlphaVantageProviderTests
    {
        [Fact]
        public async Task ObterCandlesAsync_DeveRetornarCandleFiltradoPorPeriodo()
        {
            // Arrange
            var jsonResponse = await File.ReadAllTextAsync("Mocks/AlphaVantage_WeeklyTimeSeries.json");

            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When(HttpMethod.Get, "/query*")
                    .Respond("application/json", jsonResponse);

            var httpClient = mockHttp.ToHttpClient();
            httpClient.BaseAddress = new Uri("https://www.alphavantage.co/");

            var settings = new AlphaVantageSettings { ApiKey = "FAKE_API_KEY" };
            var options = Options.Create(settings);
            var provider = new AlphaVantageProvider(httpClient, options);

            var inicio = new DateTime(2025, 06, 01);
            var fim = new DateTime(2025, 06, 30);

            // Act
            var result = await provider.ObterCandlesAsync("AAPL", inicio, fim);

            // Assert
            Assert.NotEmpty(result);
            Assert.All(result, quote =>
            {
                Assert.InRange(quote.Data, inicio, fim);
            });
        }
    }
}
