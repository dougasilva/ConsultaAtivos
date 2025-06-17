using ConsultaAtivos.Application.Dto;
using ConsultaAtivos.Domain.Entidades;

namespace ConsultaAtivos.Application.Mapper
{
    public static class BrapiMapper
    {
        public static ResumoCotacao MapToResumo(BrapiAtivoDto dto)
        {
            return new ResumoCotacao
            {
                Symbol = dto.Symbol,
                NomeCurto = dto.ShortName,
                PrecoAtual = dto.RegularMarketPrice,
                VariacaoDia = dto.RegularMarketChange,
                PercentualVariacao = dto.RegularMarketChangePercent,
                LogoUrl = dto.Logourl
            };
        }
    }
}
