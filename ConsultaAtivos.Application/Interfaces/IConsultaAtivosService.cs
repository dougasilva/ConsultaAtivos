using ConsultaAtivos.Domain.Entidades;

namespace ConsultaAtivos.Application.Interfaces
{
    public interface IConsultaAtivosService
    {
        Task<ResumoCotacao> ObterResumoAsync(string symbol, CancellationToken cancellationToken = default);
        Task<Ativo> ObterAtivoComHistoricoAsync(string symbol, string range = "5d", string interval = "1d", CancellationToken cancellationToken = default);
    }
}

