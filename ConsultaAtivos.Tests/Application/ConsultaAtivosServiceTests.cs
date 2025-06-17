using ConsultaAtivos.Application.Services;
using ConsultaAtivos.Domain.Configuration;
using Microsoft.Extensions.Options;
using Moq.Protected;
using Moq;
using System.Net;

namespace ConsultaAtivos.Tests.Application
{
    public class ConsultaAtivosServiceTests
    {
        [Fact]
        public async Task ObterResumoAsync_DeveRetornarResumoValido()
        {
            // Arrange
            var fakeJson = @"{
            ""results"": [
                {
                    ""symbol"": ""MGLU3"",
                    ""shortName"": ""MAGAZ LUIZA ON"",
                    ""regularMarketPrice"": 9.54,
                    ""regularMarketChange"": 0.6,
                    ""regularMarketChangePercent"": 6.71,
                    ""logourl"": ""https://icons.brapi.dev/icons/MGLU3.svg""
                }
            ]
        }";

            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(fakeJson)
                });

            var client = new HttpClient(handler.Object);
            var options = Options.Create(new BrapiSettings { BaseUrl = "https://brapi.dev/api", Token = "DUMMY" });

            var service = new ConsultaAtivosService(client, options);

            // Act
            var resumo = await service.ObterResumoAsync("MGLU3");

            // Assert
            Assert.Equal("MGLU3", resumo.Symbol);
            Assert.Equal("MAGAZ LUIZA ON", resumo.NomeCurto);
            Assert.Equal(9.54m, resumo.PrecoAtual);
            Assert.Equal(0.6m, resumo.VariacaoDia);
            Assert.Equal(6.71m, resumo.PercentualVariacao);
        }
    }

}
