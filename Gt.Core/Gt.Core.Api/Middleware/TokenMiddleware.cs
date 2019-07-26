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

		public Task Invoke(HttpContext context, JwtSettings jwtSettings)
		{
			var token = context.Request.Headers["Authorization"];

			context.Response.Headers["Authorization"] = "";

			// Call the next delegate/middleware in the pipeline
			return this._next(context);
		}
	}
}
