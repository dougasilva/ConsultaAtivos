namespace ConsultaAtivos.Application.Dto.BrapiRaw
{
    public class CotacaoBrapiDtoBruto
    {
        public long date { get; set; }
        public decimal open { get; set; }
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal close { get; set; }
        public long volume { get; set; }
        public decimal adjustedClose { get; set; }
    }
}
