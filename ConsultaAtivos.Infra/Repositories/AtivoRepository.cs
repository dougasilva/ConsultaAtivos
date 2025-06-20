using ConsultaAtivos.Infra.Data;
using ConsultaAtivos.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConsultaAtivos.Infra.Repositories
{
    public class AtivoRepository : IAtivoRepository
    {
        private readonly ConsultaAtivosDbContext _context;

        public AtivoRepository(ConsultaAtivosDbContext context)
        {
            _context = context;
        }

        public async Task<Ativo> ObterPorSymbolAsync(string symbol)
        {
            return await _context.Ativos.Include(a => a.Historico)
                .FirstOrDefaultAsync(a => a.Symbol == symbol);
        }

        public async Task AdicionarOuAtualizarAsync(Ativo ativo)
        {
            var existente = await _context.Ativos.FindAsync(ativo.Symbol);

            if (existente == null)
                _context.Ativos.Add(ativo);
            else
                _context.Entry(existente).CurrentValues.SetValues(ativo);

            await _context.SaveChangesAsync();
        }

        public async Task SalvarAsync(Ativo ativo)
        {
            var existente = await _context.Ativos.FindAsync(ativo.Symbol);

            if (existente == null)
            {
                await _context.Ativos.AddAsync(ativo);
            }
            else
            {
                _context.Entry(existente).CurrentValues.SetValues(ativo);
                _context.Entry(existente).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }

    }
}
