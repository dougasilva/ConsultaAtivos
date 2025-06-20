using ConsultaAtivos.Domain.Entidades;

namespace ConsultaAtivos.Application.Interfaces
{
    public interface IBrapiService
    {
        Task<Ativo> ObterAtivoComHistoricoAsync(string ticker, string range = "5d", string interval = "1d");
        Task<ResumoCotacao> ObterResumoAsync(string ticker);
    }
}
