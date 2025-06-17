public class Ativo
{
    public string Symbol { get; set; }
    public string NomeCurto { get; set; }
    public string NomeLongo { get; set; }
    public string Moeda { get; set; }
    public string LogoUrl { get; set; }
    public decimal PrecoAtual { get; set; }
    public decimal VariacaoDia { get; set; }
    public decimal PercentualVariacao { get; set; }
    public DateTime DataUltimaCotacao { get; set; }
    public List<HistoricoCotacao> Historico { get; set; }
    public decimal? PriceEarnings { get; set; }
    public decimal? EarningsPerShare { get; set; }
}
