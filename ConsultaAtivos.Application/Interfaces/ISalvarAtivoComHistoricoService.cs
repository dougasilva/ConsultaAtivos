namespace ConsultaAtivos.Application.Interfaces
{
    public interface ISalvarAtivoComHistoricoService
    {
        Task SalvarAsync(Ativo ativo);
    }
}