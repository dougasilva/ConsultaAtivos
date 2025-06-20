using AutoMapper;
using ConsultaAtivos.Application.Dto.BrapiRaw;
using ConsultaAtivos.Domain.Entidades;

namespace ConsultaAtivos.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Histórico (dados diários)
            CreateMap<CotacaoBrapiDtoBruto, HistoricoCotacao>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => DateTimeOffset.FromUnixTimeSeconds(src.date).DateTime))
                .ForMember(dest => dest.Abertura, opt => opt.MapFrom(src => src.open))
                .ForMember(dest => dest.Fechamento, opt => opt.MapFrom(src => src.close))
                .ForMember(dest => dest.Maximo, opt => opt.MapFrom(src => src.high))
                .ForMember(dest => dest.Minimo, opt => opt.MapFrom(src => src.low))
                .ForMember(dest => dest.Volume, opt => opt.MapFrom(src => src.volume));

            // Ativo completo com histórico
            CreateMap<ResultadoCotacaoCompletoBrutoDto, Ativo>()
                .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.symbol))
                .ForMember(dest => dest.NomeCurto, opt => opt.MapFrom(src => src.shortName))
                .ForMember(dest => dest.NomeLongo, opt => opt.MapFrom(src => src.longName))
                .ForMember(dest => dest.Moeda, opt => opt.MapFrom(src => src.currency))
                .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.logourl))
                .ForMember(dest => dest.PrecoAtual, opt => opt.MapFrom(src => src.regularMarketPrice))
                .ForMember(dest => dest.VariacaoDia, opt => opt.MapFrom(src => src.regularMarketChange))
                .ForMember(dest => dest.PercentualVariacao, opt => opt.MapFrom(src => src.regularMarketChangePercent))
                .ForMember(dest => dest.DataUltimaCotacao, opt => opt.MapFrom(src => DateTimeOffset.FromUnixTimeSeconds(src.regularMarketTime).DateTime))
                .ForMember(dest => dest.PriceEarnings, opt => opt.MapFrom(src => src.trailingPE))
                .ForMember(dest => dest.EarningsPerShare, opt => opt.MapFrom(src => src.epsTrailingTwelveMonths))
                .ForMember(dest => dest.Historico, opt => opt.MapFrom(src => src.historicalDataPrice));

            // Resumo simples (sem histórico nem PE/EPS)
            CreateMap<ResultadoCotacaoCompletoBrutoDto, ResumoCotacao>()
                .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.symbol))
                .ForMember(dest => dest.NomeCurto, opt => opt.MapFrom(src => src.shortName))
                .ForMember(dest => dest.PrecoAtual, opt => opt.MapFrom(src => src.regularMarketPrice))
                .ForMember(dest => dest.VariacaoDia, opt => opt.MapFrom(src => src.regularMarketChange))
                .ForMember(dest => dest.PercentualVariacao, opt => opt.MapFrom(src => src.regularMarketChangePercent))
                .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.logourl));
        }
    }
}
