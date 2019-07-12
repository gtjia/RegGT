using System;
using System.Collections.Generic;
using System.Text;

namespace Gt.Core.Model.Entities
{
	public class AppUser
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string RealName { get; set; }
		public string PictureUrl { get; set; }

		public AppRole Role { get; set; }
	}
}
