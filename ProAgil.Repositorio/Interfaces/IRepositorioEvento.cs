using System.Threading.Tasks;
using ProAgil.Dominio.Model;

namespace ProAgil.Repositorio.Interfaces
{
    public interface IRepositorioEvento
    {
        Task<Evento[]> GetAllEventoAsync(bool includePalestrantes);
		Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes);
		Task<Evento> GetEventoAsyncById(int EventoId, bool includePalestrantes);
    }
}