namespace ConsultaAtivos.Domain.Interfaces
{
    public interface IHistoricoCotacaoRepository
    {
        Task RemoverPorSymbolAsync(string symbol);
        Task AdicionarAsync(List<HistoricoCotacao> historico);
        Task SalvarTodosAsync(List<HistoricoCotacao> historicos);
    }
}