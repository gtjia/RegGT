using Gt.Core.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gt.Core.Model.Models
{
	public class UserModel
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string RealName { get; set; }
		public string PictureUrl { get; set; }

		public int RoleId { get; set; }
		public string RoleName { get; set; }
		public string[] scope { get; set; }
	}
}
