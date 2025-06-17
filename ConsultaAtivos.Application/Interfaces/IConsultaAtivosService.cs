using ConsultaAtivos.Domain.Entidades;

namespace ConsultaAtivos.Application.Interfaces
{
    public interface IConsultaAtivosService
    {
        Task<ResumoCotacao> ObterResumoAsync(string symbol, CancellationToken cancellationToken = default);
    }
}
