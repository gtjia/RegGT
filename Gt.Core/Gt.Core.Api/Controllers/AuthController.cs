using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Gt.Core.Model.Models;
using Gt.Core.Service;
using Gt.Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Gt.Core.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class AuthController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private AuthorizationService _authService;

		public AuthController(IConfiguration configuration, AuthorizationService authService)
		{
			_configuration = configuration;
			_authService = authService;
		}

		/// <summary>
		/// login
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[AllowAnonymous]
		// POST api/auth/login
		[HttpPost("login")]
		public IActionResult Login([FromBody]LoginUserModel request)
		{
			try
			{
				var token = _authService.Login(request);
				return Ok(new
				{
					token = new JwtSecurityTokenHandler().WriteToken(token)
				});
			}
			catch (BussinessException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest("Could not verify username and password");
			}
			
		}

		[HttpGet("user")]
		public IActionResult GetUser()
		{
			string userName = this.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Name).Value;

			var result = new
			{
				user = new
				{
					UserName = userName,
					scope = new string[] { "test", "admin" }
				}
			};
			return new JsonResult(result);
		}
	}
}