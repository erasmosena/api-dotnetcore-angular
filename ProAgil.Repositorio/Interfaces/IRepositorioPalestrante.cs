using System.Threading.Tasks;
using ProAgil.Dominio.Model;

namespace ProAgil.Repositorio.Interfaces
{
    public interface IRepositorioPalestrante
    {
         Task<Palestrante>GetPalestranteAsync(int PalestranteId, bool includeEventos);
		 Task<Palestrante[]>GetPalestranteAsyncByName(string nome, bool includeEventos);
    }
}