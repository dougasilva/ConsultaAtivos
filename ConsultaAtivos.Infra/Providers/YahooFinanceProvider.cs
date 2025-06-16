using YahooFinanceApi;
using ConsultaAtivos.Domain.Entities;
using ConsultaAtivos.Domain.Interfaces;

namespace ConsultaAtivos.Infra.Providers
{
    public class YahooFinanceProvider : IHistoricalQuoteProvider
    {
        public async Task<IEnumerable<HistoricalQuote>> ObterCandlesAsync(string ticker, DateTime inicio, DateTime fim)
        {
            var historico = await Yahoo.GetHistoricalAsync(ticker, inicio, fim, Period.Daily);

            return historico.Select(c => new HistoricalQuote(
                c.DateTime,
                (decimal)c.Open,
                (decimal)c.High,
                (decimal)c.Low,
                (decimal)c.Close,
                c.Volume
            ));
        }
    }
}
