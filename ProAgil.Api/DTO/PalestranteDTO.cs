using System.Collections.Generic;

namespace ProAgil.Api.DTO
{
    public class PalestranteDTO
    {
        public int PalestranteId { get; set; }
		public string Nome { get; set; }

		public string MiniCurriculo { get; set; }

		public string ImagemUrl { get; set; }

		public string Telefone { get; set; }

		public string Email { get; set; }

		public virtual IList<RedeSocialDTO> RedeSocials { get; set; }

		public virtual IList<EventoDTO> Eventos { get; set; }
    }
}