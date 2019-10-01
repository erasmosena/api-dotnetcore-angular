using System.ComponentModel.DataAnnotations;

namespace ProAgil.Api.DTO
{
	public class LoteDTO
	{


		[Required(ErrorMessage = "O campo {0} é obrigatório.")]
		public string Nome { get; set; }

		[Required(ErrorMessage = "O campo {0} é obrigatório.")]
		public decimal Preco { get; set; }

		[Required(ErrorMessage = "O campo {0} é obrigatório.")]
		public string DataInicio { get; set; }

		public string DataFim { get; set; }
		[Range(1,500,ErrorMessage="O valor do campo {0} deve ser entre {1} e {2} ")]
		public int Quantidade { get; set; }
	}
}