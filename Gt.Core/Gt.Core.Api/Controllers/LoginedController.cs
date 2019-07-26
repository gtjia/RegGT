using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gt.Core.Api.Jwt;
using Gt.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gt.Core.Api.Controllers
{
	[Authorize]
	public class LoginedController : BaseController
	{
		private UserModel _currUser;
		protected UserModel CurrentUser {
			get
			{
				if (_currUser == null)
				{
					var security = (Security)this.HttpContext.RequestServices.GetService(typeof(Security));
					_currUser = security.GetUser(this.HttpContext.Request.Headers["Authorization"]);
				}
				return _currUser;
			}
		}

		public LoginedController()
		{
			
		}
	}
}