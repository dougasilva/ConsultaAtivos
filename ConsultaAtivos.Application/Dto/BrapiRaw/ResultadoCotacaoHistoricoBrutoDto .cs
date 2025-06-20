namespace ConsultaAtivos.Application.Dto.BrapiRaw
{
    public class ResultadoCotacaoHistoricoBrutoDto
    {
        public string symbol { get; set; }
        public List<CotacaoBrapiDtoBruto> historicalDataPrice { get; set; }
    }
}
