using ConsultaAtivos.Application.Dto;
using ConsultaAtivos.Application.Interfaces;
using ConsultaAtivos.Application.Mapper;
using ConsultaAtivos.Domain.Configuration;
using ConsultaAtivos.Domain.Entidades;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ConsultaAtivos.Application.Services
{
    public class ConsultaAtivosService : IConsultaAtivosService
    {
        private readonly HttpClient _httpClient;
        private readonly BrapiSettings _settings;

        public ConsultaAtivosService(HttpClient httpClient, IOptions<BrapiSettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value;
        }

        public async Task<ResumoCotacao> ObterResumoAsync(string symbol, CancellationToken cancellationToken = default)
        {
            var url = $"{_settings.BaseUrl}/quote/{symbol}?token={_settings.Token}";
            var response = await _httpClient.GetAsync(url, cancellationToken);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var data = JsonSerializer.Deserialize<BrapiResponseDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var ativo = data?.Results?.FirstOrDefault() ?? throw new InvalidOperationException("Ativo não encontrado");

            return BrapiMapper.MapToResumo(ativo);
        }
    }
}
