using System;
using System.Collections.Generic;
using System.Text;

namespace Gt.Core.Model.Entities
{
	public class AppRole
	{
		public int Id { get; set; }
		public string RoleName { get; set; }
		public string Description { get; set; }

		public virtual ICollection<AppUser> Users { get; set; }
	}
}
