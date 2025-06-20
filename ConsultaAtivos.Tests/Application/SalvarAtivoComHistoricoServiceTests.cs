using ConsultaAtivos.Application.Services;
using ConsultaAtivos.Domain.Interfaces;
using Moq;

namespace ConsultaAtivos.Tests.Application
{
    public class SalvarAtivoComHistoricoServiceTests
    {
        [Fact]
        public async Task SalvarAtivoComHistorico_DeveSalvarAtivoEHistorico()
        {
            // Arrange
            var ativo = new Ativo
            {
                Symbol = "MGLU3",
                NomeCurto = "Magazine Luiza",
                PrecoAtual = 10.25m,
                Historico = new List<HistoricoCotacao>
        {
            new HistoricoCotacao
            {
                Data = DateTime.Today,
                Abertura = 10,
                Fechamento = 10.25m,
                Maximo = 10.4m,
                Minimo = 9.9m,
                Volume = 1000000
            }
        }
            };

            var mockAtivoRepo = new Mock<IAtivoRepository>();
            var mockHistoricoRepo = new Mock<IHistoricoCotacaoRepository>();

            var service = new SalvarAtivoComHistoricoService(mockAtivoRepo.Object, mockHistoricoRepo.Object);

            // Act
            await service.SalvarAsync(ativo);

            // Assert
            mockAtivoRepo.Verify(r => r.SalvarAsync(It.Is<Ativo>(a => a.Symbol == "MGLU3")), Times.Once);
            mockHistoricoRepo.Verify(r => r.AdicionarAsync(It.IsAny<List<HistoricoCotacao>>()), Times.Once);
        }

        [Fact]
        public async Task SalvarAtivoComHistorico_Nulo_DeveLancarExcecao()
        {
            // Arrange
            var mockAtivoRepo = new Mock<IAtivoRepository>();
            var mockHistoricoRepo = new Mock<IHistoricoCotacaoRepository>();
            var service = new SalvarAtivoComHistoricoService(mockAtivoRepo.Object, mockHistoricoRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.SalvarAsync(null));
        }

        [Fact]
        public async Task SalvarAtivoComHistorico_SemHistorico_DeveLancarExcecao()
        {
            // Arrange
            var ativo = new Ativo
            {
                Symbol = "MGLU3",
                NomeCurto = "Magazine Luiza",
                Historico = new List<HistoricoCotacao>()
            };

            var mockAtivoRepo = new Mock<IAtivoRepository>();
            var mockHistoricoRepo = new Mock<IHistoricoCotacaoRepository>();
            var service = new SalvarAtivoComHistoricoService(mockAtivoRepo.Object, mockHistoricoRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.SalvarAsync(ativo));
        }

    }
}
