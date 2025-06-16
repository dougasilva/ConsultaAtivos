using System.Text.Json;
using Microsoft.Extensions.Options;
using ConsultaAtivos.Domain.Configuration;
using ConsultaAtivos.Domain.Entities;
using ConsultaAtivos.Domain.Interfaces;

namespace ConsultaAtivos.Infra.Providers
{
    public class BrapiProvider : IHistoricalQuoteProvider
    {
        private readonly HttpClient _httpClient;
        private readonly BrapiSettings _settings;

        public BrapiProvider(HttpClient httpClient, IOptions<BrapiSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<IEnumerable<HistoricalQuote>> ObterCandlesAsync(string ticker, DateTime inicio, DateTime fim)
        {
            var url = $"https://brapi.dev/api/quote/{ticker}?range=3mo&interval=1d";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Authorization", $"Bearer {_settings.ApiKey}");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var historicalData = doc.RootElement
                .GetProperty("results")[0]
                .GetProperty("historicalDataPrice");

            var quotes = new List<HistoricalQuote>();

            foreach (var candle in historicalData.EnumerateArray())
            {
                var data = UnixTimeStampToDateTime(candle.GetProperty("date").GetInt64());

                if (data < inicio || data > fim)
                    continue;

                quotes.Add(new HistoricalQuote(
                    data,
                    candle.GetProperty("open").GetDecimal(),
                    candle.GetProperty("high").GetDecimal(),
                    candle.GetProperty("low").GetDecimal(),
                    candle.GetProperty("close").GetDecimal(),
                    candle.GetProperty("volume").GetInt64()
                ));
            }

            return quotes.OrderBy(q => q.Data);
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp);
            return dateTimeOffset.DateTime;
        }


    }
}
