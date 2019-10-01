using System.Threading.Tasks;
using ProAgil.Dominio.Model;

namespace ProAgil.Repositorio.Interfaces
{
	public interface IRepositorio
	{
		void Add<T>(T entity) where T : class;

		void Delete<T>(T entity) where T : class;

		Task<bool> SaveChangesAsync();

		void Update<T>(T entity) where T : class;

		
		Task<Evento[]> GetAllEventoAsync(bool includePalestrantes = false);
		Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes = false);
		Task<Evento> GetEventoAsyncById(int EventoId, bool includePalestrantes = false);


	
		Task<Palestrante[]> GetAllPalestranteAsync(bool includeEventos = false);
		Task<Palestrante> GetPalestranteAsync(int PalestranteId, bool includeEventos = false);
		Task<Palestrante[]> GetPalestranteAsyncByName(string nome, bool includeEventos = false);
		
	}
}