using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ProAgil.Dominio.Identity
{
	public class User : IdentityUser<int>
	{
		public string FullName { get; set; }

		public List<UserRole> UserRoles { get; set; }
	}
}