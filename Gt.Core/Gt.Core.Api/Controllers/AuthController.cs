using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Gt.Core.Api.Jwt;
using Gt.Core.Model.Models;
using Gt.Core.Service;
using Gt.Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Gt.Core.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class AuthController : ControllerBase
	{
		private Security _security;
		private AuthorizationService _authService;

		public AuthController(Security security, AuthorizationService authService)
		{
			_security = security;
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
				var user = _authService.Login(request);

				var tokenStr = _security.CreateToken(user).GenerateToken();

				return Ok(new
				{
					token = tokenStr
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
			var user = _security.GetUser(this.Request.Headers["Authorization"]);
			var result = new
			{
				user = user
			};
			return new JsonResult(result);
		}

		[AllowAnonymous]
		[HttpPost("logout")]
		public IActionResult Logout()
		{
			try
			{
				_security.RemoveToken(this.Request.Headers["Authorization"]);

				return Ok(new { });
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
	}
}