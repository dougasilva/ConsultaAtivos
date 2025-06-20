namespace ConsultaAtivos.Application.Dto.BrapiRaw
{
    public class ResultadoCotacaoCompletoBrutoDto
    {
        public string symbol { get; set; }
        public string shortName { get; set; }
        public string longName { get; set; }
        public string currency { get; set; }
        public string logourl { get; set; }
        public decimal regularMarketPrice { get; set; }
        public decimal regularMarketChange { get; set; }
        public decimal regularMarketChangePercent { get; set; }
        public long regularMarketTime { get; set; }
        public List<CotacaoBrapiDtoBruto> historicalDataPrice { get; set; }
        public decimal? trailingPE { get; set; }
        public decimal? epsTrailingTwelveMonths { get; set; }
    }
}
