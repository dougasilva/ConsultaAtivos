namespace ConsultaAtivos.Domain.Entities
{
    public class HistoricalQuote
    {
        public DateTime Data { get; }
        public decimal Abertura { get; }
        public decimal Maxima { get; }
        public decimal Minima { get; }
        public decimal Fechamento { get; }
        public long Volume { get; }

        public HistoricalQuote(DateTime data, decimal abertura, decimal maxima, decimal minima, decimal fechamento, long volume)
        {
            Data = data;
            Abertura = abertura;
            Maxima = maxima;
            Minima = minima;
            Fechamento = fechamento;
            Volume = volume;
        }
    }
}
