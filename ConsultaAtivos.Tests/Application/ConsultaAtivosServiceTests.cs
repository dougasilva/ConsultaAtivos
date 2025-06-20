using ConsultaAtivos.Application.Interfaces;
using ConsultaAtivos.Application.Services;
using ConsultaAtivos.Domain.Entidades;
using Moq;
using Xunit;

namespace ConsultaAtivos.Tests.Application
{
    public class ConsultaAtivosServiceTests
    {
        [Fact]
        public async Task ObterResumoAsync_DeveRetornarResumoValido()
        {
            // Arrange
            var mockBrapiService = new Mock<IBrapiService>();

            var resumoMock = new ResumoCotacao
            {
                Symbol = "MGLU3",
                NomeCurto = "MAGAZ LUIZA ON",
                PrecoAtual = 9.54m,
                VariacaoDia = 0.6m,
                PercentualVariacao = 6.71m,
                LogoUrl = "https://icons.brapi.dev/icons/MGLU3.svg"
            };

            mockBrapiService
                .Setup(s => s.ObterResumoAsync("MGLU3"))
                .ReturnsAsync(resumoMock);

            var service = new ConsultaAtivosService(mockBrapiService.Object);

            // Act
            var resumo = await service.ObterResumoAsync("MGLU3");

            // Assert
            Assert.Equal("MGLU3", resumo.Symbol);
            Assert.Equal("MAGAZ LUIZA ON", resumo.NomeCurto);
            Assert.Equal(9.54m, resumo.PrecoAtual);
            Assert.Equal(0.6m, resumo.VariacaoDia);
            Assert.Equal(6.71m, resumo.PercentualVariacao);
            Assert.Equal("https://icons.brapi.dev/icons/MGLU3.svg", resumo.LogoUrl);
        }

        [Fact]
        public async Task ObterAtivoComHistoricoAsync_DeveRetornarAtivoCompletoComHistorico()
        {
            // Arrange
            var mockBrapiService = new Mock<IBrapiService>();

            var ativoMock = new Ativo
            {
                Symbol = "PETR4",
                NomeCurto = "PETROBRAS PN",
                NomeLongo = "PETROLEO BRASILEIRO SA PETROBRAS",
                Moeda = "BRL",
                LogoUrl = "https://icons.brapi.dev/icons/PETR4.svg",
                PrecoAtual = 33.50m,
                VariacaoDia = 0.75m,
                PercentualVariacao = 2.29m,
                DataUltimaCotacao = DateTime.Now,
                PriceEarnings = 4.2m,
                EarningsPerShare = 7.89m,
                Historico = new List<HistoricoCotacao>
        {
            new HistoricoCotacao
            {
                Data = DateTime.Today.AddDays(-1),
                Abertura = 33.00m,
                Fechamento = 33.50m,
                Maximo = 34.00m,
                Minimo = 32.80m,
                Volume = 10000000
            }
        }
            };

            mockBrapiService
                .Setup(s => s.ObterAtivoComHistoricoAsync("PETR4", "5d", "1d"))
                .ReturnsAsync(ativoMock);

            var service = new ConsultaAtivosService(mockBrapiService.Object);

            // Act
            var ativo = await service.ObterAtivoComHistoricoAsync("PETR4");

            // Assert
            Assert.Equal("PETR4", ativo.Symbol);
            Assert.Equal("PETROBRAS PN", ativo.NomeCurto);
            Assert.Equal("PETROLEO BRASILEIRO SA PETROBRAS", ativo.NomeLongo);
            Assert.Equal(33.50m, ativo.PrecoAtual);
            Assert.NotEmpty(ativo.Historico);
            Assert.Equal(33.00m, ativo.Historico[0].Abertura);
        }

        [Fact]
        public async Task ObterResumoAsync_DeveLancarExcecao_SeNaoEncontrarAtivo()
        {
            var mock = new Mock<IBrapiService>();
            mock.Setup(s => s.ObterResumoAsync(It.IsAny<string>()))
                .ThrowsAsync(new InvalidOperationException("Ativo não encontrado"));

            var service = new ConsultaAtivosService(mock.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.ObterResumoAsync("XXXX"));
        }

        [Fact]
        public async Task ObterAtivoComHistoricoAsync_DeveRetornarHistoricoVazio_SeNaoHouverDados()
        {
            var mock = new Mock<IBrapiService>();
            var ativoMock = new Ativo
            {
                Symbol = "VALE3",
                Historico = new List<HistoricoCotacao>() // Sem histórico
            };

            mock.Setup(s => s.ObterAtivoComHistoricoAsync("VALE3", "5d", "1d"))
                .ReturnsAsync(ativoMock);

            var service = new ConsultaAtivosService(mock.Object);
            var ativo = await service.ObterAtivoComHistoricoAsync("VALE3");

            Assert.Empty(ativo.Historico);
        }


    }
}
