using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ProAgil.Api.DTO
{
	public class EventoDTO
	{
		[JsonProperty("idEvento")]
		public int EventoId { get; set; }

		[Required(ErrorMessage = "O campo {0} é obrigatório.")]
		[StringLength(50,MinimumLength= 3,ErrorMessage="O valor do campo {0} deve ser entre {2} e {1} Caracteres")]
		public string Local { get; set; }
		public string DataEvento { get; set; }

		[Required(ErrorMessage = "O campo {0} é obrigatório.")]
		[StringLength(50,MinimumLength= 3,ErrorMessage="O valor do campo {0} deve ser entre {2} e {1} Caracteres")]
		public string Tema { get; set; }
		
		[Range(2,5000,ErrorMessage="O valor do campo {0} deve ser entre {1} e {2} ")]
		public int QtdPessoas { get; set; }
		public string ImagemURL { get; set; }

		[Phone(ErrorMessage = "O campo {0} é do tipo Telefone, está em formato inválido.")]	
		public string Telefone { get; set; }

		[EmailAddress(ErrorMessage = "E-mail em formato inválido.")]
		public string Email { get; set; }
		public virtual IList<LoteDTO> Lotes { get; set; }

		public virtual IList<RedeSocialDTO> RedeSocials { get; set; }

		public virtual IList<PalestranteDTO> Palestrantes { get; set; }
	}
}