using System.ComponentModel.DataAnnotations;

namespace ProAgil.Api.DTO
{
	public class LoginDTO
	{
		[Required(ErrorMessage = "O campo {0} é obrigatório.")]
		[StringLength(50, ErrorMessage = "O campo {0} deve conter entre {2} e {1} caracteres.", MinimumLength = 4)]
		public string UserName { get; set; }

		[Required(ErrorMessage = "O campo {0} é obritatório")]
		[StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
		public string Password { get; set; }
	}
}