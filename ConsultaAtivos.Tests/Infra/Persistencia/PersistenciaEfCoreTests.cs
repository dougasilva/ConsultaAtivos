using ConsultaAtivos.Application.Services;
using ConsultaAtivos.Domain.Entidades;
using ConsultaAtivos.Infra.Data;
using ConsultaAtivos.Infra.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ConsultaAtivos.Tests.Infra
{
    public class PersistenciaEfCoreTests
    {
        private static DbContextOptions<ConsultaAtivosDbContext> CriarOpcoes()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            return new DbContextOptionsBuilder<ConsultaAtivosDbContext>()
                .UseSqlite(connection)
                .Options;
        }

        [Fact]
        public async Task DeveSalvarAtivoESeusHistoricos()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open(); // Mantém conexão viva durante o teste

            var options = new DbContextOptionsBuilder<ConsultaAtivosDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new ConsultaAtivosDbContext(options))
            {
                context.Database.EnsureCreated();

                var ativoRepo = new AtivoRepository(context);
                var historicoRepo = new HistoricoCotacaoRepository(context);
                var service = new SalvarAtivoComHistoricoService(ativoRepo, historicoRepo);

                var ativo = new Ativo
                {
                    Symbol = "MGLU3",
                    NomeCurto = "Magazine Luiza",
                    NomeLongo = "Magazine Luiza S.A.",
                    Moeda = "Real",
                    LogoUrl = "https://icons.brapi.dev/icons/MGLU3.svg",
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
                    Data = DateTime.Today,
                    Abertura = 10,
                    Fechamento = 10.25m,
                    Maximo = 10.4m,
                    Minimo = 9.9m,
                    Volume = 1000000
                }
            }
                };

                await service.SalvarAsync(ativo);
            }

            using (var context = new ConsultaAtivosDbContext(options))
            {
                var ativoDb = await context.Ativos
                    .Include(a => a.Historico)
                    .FirstOrDefaultAsync(a => a.Symbol == "MGLU3");

                Assert.NotNull(ativoDb);
                Assert.Single(ativoDb.Historico);
                Assert.Equal(33.50m, ativoDb.PrecoAtual);
            }

            connection.Close(); // Fecha após o teste completo
        }

    }
}
