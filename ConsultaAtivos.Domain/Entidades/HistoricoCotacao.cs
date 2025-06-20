public class HistoricoCotacao
{
    public DateTime Data { get; set; }
    public decimal Abertura { get; set; }
    public decimal Fechamento { get; set; }
    public decimal Maximo { get; set; }
    public decimal Minimo { get; set; }
    public long Volume { get; set; }

    public string AtivoSymbol { get; set; }
    public Ativo Ativo { get; set; }
}
