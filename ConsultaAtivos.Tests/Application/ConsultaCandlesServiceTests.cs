using Moq;
using Xunit;
using FluentAssertions;
using ConsultaAtivos.Application.Services;
using ConsultaAtivos.Domain.Entities;
using ConsultaAtivos.Domain.Interfaces;

namespace ConsultaAtivos.Tests.Application
{
    public class ConsultaCandlesServiceTests
    {
        [Fact]
        public async Task Deve_Retornar_Candles_Para_O_Ticker_E_Periodo()
        {
            // Arrange
            var repositoryMock = new Mock<IHistoricalQuoteProvider>();

            var candlesEsperados = new List<HistoricalQuote>
            {
                new HistoricalQuote(new DateTime(2024, 01, 01), 10, 12, 9, 11, 100000),
                new HistoricalQuote(new DateTime(2024, 01, 02), 11, 13, 10, 12, 200000)
            };

            repositoryMock
                .Setup(r => r.ObterCandlesAsync("AAPL", new DateTime(2024, 01, 01), new DateTime(2024, 01, 02)))
                .ReturnsAsync(candlesEsperados);

            var service = new ConsultaHistoricalQuoteService(repositoryMock.Object);

            // Act
            var resultado = await service.ConsultarAsync("AAPL", new DateTime(2024, 01, 01), new DateTime(2024, 01, 02));

            // Assert
            resultado.Should().BeEquivalentTo(candlesEsperados);
        }

        [Fact]
        public async Task Deve_Lancar_ArgumentException_Se_Ticker_Estiver_Vazio()
        {
            // Arrange
            var providerMock = new Mock<IHistoricalQuoteProvider>();
            var service = new ConsultaHistoricalQuoteService(providerMock.Object);

            // Act
            Func<Task> acao = async () => await service.ConsultarAsync("", DateTime.Today, DateTime.Today);

            // Assert
            await acao.Should().ThrowAsync<ArgumentException>().WithMessage("Ticker inválido");
        }

        [Fact]
        public async Task Deve_Lancar_ArgumentException_Se_Data_Inicio_Maior_Que_Fim()
        {
            // Arrange
            var repositoryMock = new Mock<IHistoricalQuoteProvider>();
            var service = new ConsultaHistoricalQuoteService(repositoryMock.Object);

            var inicio = DateTime.Today;
            var fim = DateTime.Today.AddDays(-1);

            // Act
            Func<Task> acao = async () => await service.ConsultarAsync("AAPL", inicio, fim);

            // Assert
            await acao.Should().ThrowAsync<ArgumentException>().WithMessage("Intervalo de datas inválido");
        }

    }
}
