using Gt.Core.Api.Jwt;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gt.Core.Api.Middleware
{
	public class TokenMiddleware
	{
		private readonly RequestDelegate _next;

		public TokenMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public Task Invoke(HttpContext context, Security security)
		{
			var token = context.Request.Headers["Authorization"];
			TokenDto tokenDto = null;
			if (!string.IsNullOrWhiteSpace(token))
			{
				tokenDto = security.GetExistenceToken(token);
				if (tokenDto != null && tokenDto.IsTokenExpired)
				{
					if (tokenDto.CanRefreshToken)
					{
						var newtoken = tokenDto.GenerateToken();
						context.Response.Headers["refresh-token"] = newtoken;
					}
					else
					{
						security.RemoveToken(tokenDto);
						tokenDto = null;
					}
				}
			}

			// Call the next delegate/middleware in the pipeline
			var req = this._next(context);

			if (tokenDto != null)
				tokenDto.LastAccess = DateTime.Now;

			return req;
		}
	}
}
