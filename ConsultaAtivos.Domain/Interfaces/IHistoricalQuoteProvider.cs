using ConsultaAtivos.Domain.Entities;

namespace ConsultaAtivos.Domain.Interfaces
{
    public interface IHistoricalQuoteProvider
    {
        Task<IEnumerable<HistoricalQuote>> ObterCandlesAsync(string ticker, DateTime inicio, DateTime fim);
    }
}
