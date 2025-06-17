namespace ConsultaAtivos.Domain.Entidades
{
    public class ResumoCotacao
    {
        public string Symbol { get; set; }
        public string NomeCurto { get; set; }
        public decimal PrecoAtual { get; set; }
        public decimal VariacaoDia { get; set; }
        public decimal PercentualVariacao { get; set; }
        public string LogoUrl { get; set; }
    }

}
