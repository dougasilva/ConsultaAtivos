using ConsultaAtivos.Domain.Configuration;
using ConsultaAtivos.Domain.Entities;
using ConsultaAtivos.Domain.Interfaces;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Text.Json;

namespace ConsultaAtivos.Infra.Providers
{
    public class AlphaVantageProvider : IHistoricalQuoteProvider
    {
        private readonly HttpClient _httpClient;
        private readonly AlphaVantageSettings _settings;

        public AlphaVantageProvider(HttpClient httpClient, IOptions<AlphaVantageSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<IEnumerable<HistoricalQuote>> ObterCandlesAsync(string ticker, DateTime inicio, DateTime fim)
        {
            string url = $"query?function=TIME_SERIES_WEEKLY&symbol={ticker}&apikey={_settings.ApiKey}&outputsize=full";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("Weekly Time Series", out var timeSeries))
                throw new InvalidOperationException("AlphaVantage retornou resposta inválida.");

            var quotes = new List<HistoricalQuote>();

            foreach (var candle in timeSeries.EnumerateObject())
            {
                var data = DateTime.Parse(candle.Name);
                if (data < inicio || data > fim) continue;

                var valores = candle.Value;

                var open = ParseDecimal(valores, "1. open");
                var high = ParseDecimal(valores, "2. high");
                var low = ParseDecimal(valores, "3. low");
                var close = ParseDecimal(valores, "4. close");
                var volume = ParseLong(valores, "5. volume");

                quotes.Add(new HistoricalQuote(data, open, high, low, close, volume));
            }

            return quotes.OrderBy(q => q.Data);
        }

        private decimal ParseDecimal(JsonElement element, string property)
        {
            var value = element.GetProperty(property).GetString();
            return decimal.Parse(value, CultureInfo.InvariantCulture);
        }

        private long ParseLong(JsonElement element, string property)
        {
            var value = element.GetProperty(property).GetString();
            return long.Parse(value, CultureInfo.InvariantCulture);
        }
    }
}
