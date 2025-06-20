namespace ConsultaAtivos.Domain.Interfaces
{
    public interface IAtivoRepository
    {
        Task<Ativo?> ObterPorSymbolAsync(string symbol);
        Task AdicionarOuAtualizarAsync(Ativo ativo);
        Task SalvarAsync(Ativo ativo);
    }

}
