using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using ProAgil.Api.DTO;
using ProAgil.Dominio.Identity;

namespace ProAgil.Api.Controllers
{
	[Route("api/[Controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly TokenConfigurations _tokenConfig;
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IMapper _mapper;
		public UsersController(IOptions<TokenConfigurations> tokenConfig, UserManager<User> userManager
			, SignInManager<User> signInManager, IMapper mapper)
		{
			_tokenConfig = tokenConfig.Value;
			_userManager = userManager;
			_signInManager = signInManager;
			_mapper = mapper;

		}


		// GET api/Users
		[HttpGet]

		public async Task<IActionResult> Get()
		{
			try
			{
				return Ok(new UserDTO());
			}
			catch (System.Exception ex)
			{

				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}

		}

		// Post api/Users
		[HttpPost("{action}")]
		[AllowAnonymous]
		public async Task<IActionResult> Register(UserDTO dto)
		{
			try
			{
				var user = _mapper.Map<User>(dto);
				var result = await _userManager.CreateAsync(user, dto.Password);

				if (result.Succeeded)
				{
					var userToReturn = _mapper.Map<UserDTO>(user);
					return Created("Get", userToReturn);
				}
				return BadRequest(result.Errors);
			}
			catch (System.Exception ex)
			{

				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}

		}

		// Post api/Users
		[HttpPost("{action}")]
		[AllowAnonymous]
		public async Task<IActionResult> Login(LoginDTO dto)
		{
			try
			{
				var user = await _userManager.FindByNameAsync(dto.UserName);
				if (user != null)
				{
					var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
					if (result.Succeeded)
					{
						var appuser = await _userManager.Users
							.FirstOrDefaultAsync(u => u.NormalizedUserName == dto.UserName.ToUpper());
						var userToReturn = _mapper.Map<LoginDTO>(appuser);
						return Ok(
							new
							{
								token = GenerateJwtToken(appuser).Result,
								user = userToReturn
							}
						);
					}
				}
				return Unauthorized();

			}
			catch (System.Exception ex)
			{

				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}

		}

		private async Task<string> GenerateJwtToken(User user)
		{
			var claims = new List<Claim>(){
				new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
				new Claim(ClaimTypes.Name, user.UserName)
			};

			var roles = await _userManager.GetRolesAsync(user);
			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenConfig.Secret));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddHours(Convert.ToInt32(_tokenConfig.ExpiracaoHoras)),
				SigningCredentials = creds
			};
			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

	}
}