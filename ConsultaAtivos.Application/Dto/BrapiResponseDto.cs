namespace ConsultaAtivos.Application.Dto
{
    public class BrapiResponseDto
    {
        public List<BrapiAtivoDto> Results { get; set; }
        public string RequestedAt { get; set; }
    }
}
