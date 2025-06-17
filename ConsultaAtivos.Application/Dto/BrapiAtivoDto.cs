namespace ConsultaAtivos.Application.Dto
{
    public class BrapiAtivoDto
    {
        public string Symbol { get; set; }
        public string ShortName { get; set; }
        public decimal RegularMarketPrice { get; set; }
        public decimal RegularMarketChange { get; set; }
        public decimal RegularMarketChangePercent { get; set; }
        public string Logourl { get; set; }
    }
}
