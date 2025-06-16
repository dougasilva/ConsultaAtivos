using ConsultaAtivos.Domain.Entities;

namespace ConsultaAtivos.Domain.Interfaces
{
    public interface IBrapiProvider
    {
        Task<IEnumerable<HistoricalQuote>> ObterHistoricoAsync(string ticker, DateTime inicio, DateTime fim);
    }
}
