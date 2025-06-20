using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultaAtivos.Application.Dto.BrapiRaw
{
    public class RespostaAtivoBrutoDto
    {
        public List<ResultadoCotacaoCompletoBrutoDto> results { get; set; }
    }
}
