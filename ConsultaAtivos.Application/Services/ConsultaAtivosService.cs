using ConsultaAtivos.Application.Interfaces;
using ConsultaAtivos.Domain.Entidades;

namespace ConsultaAtivos.Application.Services
{
    public class ConsultaAtivosService : IConsultaAtivosService
    {
        private readonly IBrapiService _brapiService;

        public ConsultaAtivosService(IBrapiService brapiService)
        {
            _brapiService = brapiService;
        }

        public async Task<ResumoCotacao> ObterResumoAsync(string symbol, CancellationToken cancellationToken = default)
        {
            return await _brapiService.ObterResumoAsync(symbol);
        }

        public async Task<Ativo> ObterAtivoComHistoricoAsync(string symbol, string range = "5d", string interval = "1d", CancellationToken cancellationToken = default)
        {
            return await _brapiService.ObterAtivoComHistoricoAsync(symbol, range, interval);
        }
    }
}
