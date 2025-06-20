using ConsultaAtivos.Application.Interfaces;
using ConsultaAtivos.Domain.Interfaces;

namespace ConsultaAtivos.Application.Services
{
    public class SalvarAtivoComHistoricoService : ISalvarAtivoComHistoricoService
    {
        private readonly IAtivoRepository _ativoRepository;
        private readonly IHistoricoCotacaoRepository _historicoRepository;

        public SalvarAtivoComHistoricoService(IAtivoRepository ativoRepository, IHistoricoCotacaoRepository historicoRepository)
        {
            _ativoRepository = ativoRepository;
            _historicoRepository = historicoRepository;
        }

        public async Task SalvarAsync(Ativo ativo)
        {
            if (ativo == null)
                throw new ArgumentNullException(nameof(ativo));

            if (ativo.Historico == null || !ativo.Historico.Any())
                throw new InvalidOperationException("Histórico de cotações não pode estar vazio.");

            // Força associação correta
            ativo.Historico.ForEach(h => h.AtivoSymbol = ativo.Symbol);

            // Remove históricos antigos (se existirem)
            await _historicoRepository.RemoverPorSymbolAsync(ativo.Symbol);

            // Desconecta os históricos antes de salvar o ativo
            var historicoTemp = ativo.Historico;
            ativo.Historico = null;

            // Salva ou atualiza o ativo
            await _ativoRepository.SalvarAsync(ativo);

            // Agora salva os históricos com chave composta garantida
            await _historicoRepository.AdicionarAsync(historicoTemp);
        }
    }
}
