using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ProAgil.Dominio.Model
{
	public class Evento
	{
		[JsonProperty("idEvento")]
		public int EventoId { get; set; }
		public string Local { get; set; }
		public DateTime DataEvento { get; set; }
		public string Tema { get; set; }
		public int QtdPessoas { get; set; }
		public string ImagemURL { get; set; }

		public string Telefone { get; set; }

		public string Email{ get; set; }
		public virtual IList<Lote> Lotes { get; set; }

		public virtual IList<RedeSocial> RedeSocials { get; set; }

		public virtual IList<PalestranteEvento> PalestranteEventos { get; set; }
	}
}