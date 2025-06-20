using AutoMapper;
using ConsultaAtivos.Application.Dto.BrapiRaw;
using ConsultaAtivos.Domain.Configuration;
using ConsultaAtivos.Domain.Entidades;
using ConsultaAtivos.Application.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ConsultaAtivos.Infra.Services
{
    public class BrapiService : IBrapiService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly BrapiSettings _settings;

        public BrapiService(HttpClient httpClient, IMapper mapper, IOptions<BrapiSettings> options)
        {
            _httpClient = httpClient;
            _mapper = mapper;
            _settings = options.Value;
        }

        public async Task<Ativo> ObterAtivoComHistoricoAsync(string ticker, string range = "5d", string interval = "1d")
        {
            var url = $"{_settings.BaseUrl}/quote/{ticker}?range={range}&interval={interval}&fundamental=true&token={_settings.Token}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var bruto = JsonConvert.DeserializeObject<RespostaAtivoBrutoDto>(json);
            var resultado = bruto?.results?.FirstOrDefault();

            return resultado != null
                ? _mapper.Map<Ativo>(resultado)
                : throw new InvalidOperationException("Ativo não encontrado");
        }

        public async Task<ResumoCotacao> ObterResumoAsync(string ticker)
        {
            // Mesmo endpoint com fundamental=true (única fonte agora)
            var url = $"{_settings.BaseUrl}/quote/{ticker}?range=1d&interval=1d&fundamental=true&token={_settings.Token}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var bruto = JsonConvert.DeserializeObject<RespostaAtivoBrutoDto>(json);
            var resultado = bruto?.results?.FirstOrDefault();

            return resultado != null
                ? _mapper.Map<ResumoCotacao>(resultado)
                : throw new InvalidOperationException("Resumo não encontrado");
        }
    }
}
