using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Dominio.Model;
using ProAgil.Repositorio.Interfaces;

namespace ProAgil.Repositorio
{
	public class Repositorio : IRepositorio
	{
		private readonly ProAgilContext _proAgilContext;
		public Repositorio(ProAgilContext proAgilContext)
		{
			_proAgilContext = proAgilContext;
		}
		public void Add<T>(T entity) where T : class
		{
			_proAgilContext.Add(entity);
		}

		public void Delete<T>(T entity) where T : class
		{
			_proAgilContext.Remove(entity);
		}

		public async Task<Evento[]> GetAllEventoAsync(bool includePalestrantes = false)
		{
			IQueryable<Evento> query = _proAgilContext.Eventos
			.Include( l => l.Lotes)
			.Include(rs => rs.RedeSocials);
			if( includePalestrantes){
				query = query
					.Include(pe => pe.PalestranteEventos)
					.ThenInclude(p => p.Palestrante);
			}
			query = query
				.AsNoTracking()
				.OrderByDescending( c => c.DataEvento);
			return await query.ToArrayAsync();
		}

		public async Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes = false)
		{
			IQueryable<Evento> query = _proAgilContext.Eventos
			.Include( l => l.Lotes)
			.Include(rs => rs.RedeSocials);
			if( includePalestrantes){
				query = query
					.Include(pe => pe.PalestranteEventos)
					.ThenInclude(p => p.Palestrante);
			}
			query = query
				.AsNoTracking()
				.Where(it=>it.Tema.ToLower().Contains(tema.ToLower()))
				.OrderByDescending( c => c.DataEvento);
			
			return await query.ToArrayAsync();
		}

		public async Task<Palestrante[]> GetAllPalestranteAsync(bool includeEventos = false)
		{
			IQueryable<Palestrante> query = _proAgilContext.Palestrantes
			.Include(rs => rs.RedeSocials);
			if( includeEventos){
				query = query
					.Include(pe => pe.PalestranteEventos)
					.ThenInclude(e => e.Evento);
			}
			query = query
				.AsNoTracking()
				.OrderBy( c => c.Nome);
			return await query.ToArrayAsync();
		}

		public async Task<Evento> GetEventoAsyncById(int EventoId, bool includePalestrantes = false)
		{
			IQueryable<Evento> query = _proAgilContext.Eventos
			.Include( l => l.Lotes)
			.Include(rs => rs.RedeSocials);
			if( includePalestrantes){
				query = query
					.Include(pe => pe.PalestranteEventos)
					.ThenInclude(p => p.Palestrante);
			}
			query = query
				.AsNoTracking()
				.Where(it=>it.EventoId == EventoId );
				
			
			return await query.FirstOrDefaultAsync();
		}

		public async Task<Palestrante> GetPalestranteAsync(int PalestranteId, bool includeEventos = false)
		{
			IQueryable<Palestrante> query = _proAgilContext.Palestrantes
			.Include(rs => rs.RedeSocials);
			if( includeEventos){
				query = query
					.Include(pe => pe.PalestranteEventos)
					.ThenInclude(e => e.Evento);
			}
			query = query
				.AsNoTracking()
				.Where(it=>it.PalestranteId == PalestranteId );
				
			
			return await query.FirstOrDefaultAsync();
		}

		public async Task<Palestrante[]> GetPalestranteAsyncByName(string nome , bool includeEventos = false)
		{
			IQueryable<Palestrante> query = _proAgilContext.Palestrantes
			.Include(rs => rs.RedeSocials);
			if( includeEventos){
				query = query
					.Include(pe => pe.PalestranteEventos)
					.ThenInclude(e => e.Evento);
			}
			query = query
				.AsNoTracking()
				.Where(it=>it.Nome.ToLower().Contains(nome.ToLower()));
			
			return await query.ToArrayAsync();
		}

		public async Task<bool> SaveChangesAsync()
		{
			return await _proAgilContext.SaveChangesAsync() > 0 ;
		}

		public void Update<T>(T entity) where T : class
		{
			_proAgilContext.Update(entity);
		}
	}
}