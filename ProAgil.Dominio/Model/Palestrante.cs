using System.Collections.Generic;

namespace ProAgil.Dominio.Model
{
	public class Palestrante
	{
		public int PalestranteId { get; set; }
		public string Nome { get; set; }

		public string MiniCurriculo { get; set; }

		public string ImagemUrl { get; set; }

		public string Telefone { get; set; }

		public string Email { get; set; }

		public virtual IList<RedeSocial> RedeSocials { get; set; }

		public virtual IList<PalestranteEvento> PalestranteEventos { get; set; }
	}
}