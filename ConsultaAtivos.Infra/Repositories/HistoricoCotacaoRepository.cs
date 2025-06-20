using ConsultaAtivos.Infra.Data;
using ConsultaAtivos.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConsultaAtivos.Infra.Repositories
{
    public class HistoricoCotacaoRepository : IHistoricoCotacaoRepository
    {
        private readonly ConsultaAtivosDbContext  _context;

        public HistoricoCotacaoRepository(ConsultaAtivosDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HistoricoCotacao>> ObterPorSymbolAsync(string symbol)
        {
            return await _context.Historicos
                .Where(h => EF.Property<string>(h, "AtivoSymbol") == symbol)
                .ToListAsync();
        }

        public async Task AdicionarVariosAsync(List<HistoricoCotacao> historicos)
        {
            _context.Historicos.AddRange(historicos);
            await _context.SaveChangesAsync();
        }

        public async Task SalvarTodosAsync(List<HistoricoCotacao> historicos)
        {
            var symbols = historicos.Select(h => h.AtivoSymbol).Distinct().ToList();

            var datas = historicos.Select(h => h.Data).Distinct().ToList();

            var existentes = await _context.Historicos
                .Where(h => symbols.Contains(h.AtivoSymbol) && datas.Contains(h.Data))
                .ToListAsync();

            var novos = historicos
                .Where(h => !existentes.Any(e => e.AtivoSymbol == h.AtivoSymbol && e.Data == h.Data))
                .ToList();

            await _context.Historicos.AddRangeAsync(novos);
            await _context.SaveChangesAsync();
        }

        public async Task AdicionarAsync(List<HistoricoCotacao> historico)
        {
            await _context.Historicos.AddRangeAsync(historico);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverPorSymbolAsync(string symbol)
        {
            var historicos = await _context.Historicos
            .Where(h => h.AtivoSymbol == symbol)
            .ToListAsync();

            if (historicos.Any())
            {
                _context.Historicos.RemoveRange(historicos);
                await _context.SaveChangesAsync(); // ESSENCIAL
            }
        }
    }
}
