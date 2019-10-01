using System.Linq;
using AutoMapper;
using ProAgil.Api.DTO;
using ProAgil.Dominio.Identity;
using ProAgil.Dominio.Model;

namespace ProAgil.Api.Helpers
{
	public class AutoMapperProfiles : Profile
	{
		public AutoMapperProfiles()
		{
			CreateMap<Evento, EventoDTO>()
				.ForMember(dest => dest.Palestrantes, opt =>
				{
					opt.MapFrom(src => src.PalestranteEventos.Select(it => it.Palestrante).ToList());
				}).ReverseMap();
			CreateMap<Palestrante, PalestranteDTO>()
				.ForMember( dest => dest.Eventos, opt =>{
					opt.MapFrom( src => src.PalestranteEventos.Select(it => it.Evento).ToList());
				}).ReverseMap();
			CreateMap<Lote, LoteDTO>().ReverseMap();
			CreateMap<RedeSocialDTO, RedeSocialDTO>().ReverseMap();
			CreateMap<User,UserDTO>().ReverseMap();
			CreateMap<User,LoginDTO>().ReverseMap();
		}
	}
}