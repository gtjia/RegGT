using Gt.Core.Model.Entities;
using Gt.Core.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gt.Core.Api.Jwt
{
	public class TokenDto
	{
		public string Token { get; set; }
		public UserModel User { get; set; }
		public DateTime LastAccess { get; set; }
	}
}
