using ConsultaAtivos.Domain.Configuration;
using ConsultaAtivos.Infra.Services;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using AutoMapper;
using Xunit;

namespace ConsultaAtivos.Tests.Infra
{
    public class BrapiServiceTests
    {
        private readonly IMapper _mapper;

        public BrapiServiceTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ConsultaAtivos.Application.Mapping.MappingProfile>());
            _mapper = config.CreateMapper();
        }

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
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(fakeJson, Encoding.UTF8, "application/json")
                });

            var client = new HttpClient(handler.Object);
            var settings = Options.Create(new BrapiSettings { BaseUrl = "https://brapi.dev/api", Token = "DUMMY" });

            var service = new BrapiService(client, _mapper, settings);

            // Act
            var resumo = await service.ObterResumoAsync("MGLU3");

            // Assert
            Assert.NotNull(resumo);
            Assert.Equal("MGLU3", resumo.Symbol);
            Assert.Equal("MAGAZ LUIZA ON", resumo.NomeCurto);
            Assert.Equal(9.54m, resumo.PrecoAtual);
            Assert.Equal(0.6m, resumo.VariacaoDia);
            Assert.Equal(6.71m, resumo.PercentualVariacao);
            Assert.Equal("https://icons.brapi.dev/icons/MGLU3.svg", resumo.LogoUrl);
        }

        [Fact]
        public async Task ObterAtivoComHistoricoAsync_DeveRetornarAtivoCompleto()
        {
            // Arrange
            var fakeJson = @"{
                ""results"": [
                    {
                        ""symbol"": ""MGLU3"",
                        ""shortName"": ""MAGAZ LUIZA ON"",
                        ""longName"": ""Magazine Luiza S.A."",
                        ""currency"": ""BRL"",
                        ""logourl"": ""https://icons.brapi.dev/icons/MGLU3.svg"",
                        ""regularMarketPrice"": 9.54,
                        ""regularMarketChange"": 0.6,
                        ""regularMarketChangePercent"": 6.71,
                        ""regularMarketTime"": 1718650800,
                        ""priceEarnings"": 15.32,
                        ""earningsPerShare"": 0.62,
                        ""historicalDataPrice"": [
                            {
                                ""date"": 1718564400,
                                ""open"": 9.2,
                                ""high"": 9.7,
                                ""low"": 9.1,
                                ""close"": 9.54,
                                ""volume"": 23450000,
                                ""adjustedClose"": 9.54
                            }
                        ]
                    }
                ]
            }";

            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(fakeJson, Encoding.UTF8, "application/json")
                });

            var client = new HttpClient(handler.Object);
            var settings = Options.Create(new BrapiSettings { BaseUrl = "https://brapi.dev/api", Token = "DUMMY" });

            var service = new BrapiService(client, _mapper, settings);

            // Act
            var ativo = await service.ObterAtivoComHistoricoAsync("MGLU3");

            // Assert
            Assert.NotNull(ativo);
            Assert.Equal("MGLU3", ativo.Symbol);
            Assert.Equal("Magazine Luiza S.A.", ativo.NomeLongo);
            Assert.Single(ativo.Historico);
            Assert.Equal(9.2m, ativo.Historico.First().Abertura);
        }
    }
}
