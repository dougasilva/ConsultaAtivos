using ConsultaAtivos.Domain.Entities;
using ConsultaAtivos.Domain.Interfaces;

namespace ConsultaAtivos.Application.Services
{
    public class ConsultaHistoricalQuoteService
    {
        private readonly IHistoricalQuoteProvider _provider;

        public ConsultaHistoricalQuoteService(IHistoricalQuoteProvider provider)
        {
            _provider = provider;
        }

        public async Task<IEnumerable<HistoricalQuote>> ConsultarAsync(string ticker, DateTime inicio, DateTime fim)
        {
            if (string.IsNullOrWhiteSpace(ticker))
                throw new ArgumentException("Ticker inválido");

            if (inicio > fim)
                throw new ArgumentException("Intervalo de datas inválido");

            return await _provider.ObterCandlesAsync(ticker, inicio, fim);
        }
    }
}
