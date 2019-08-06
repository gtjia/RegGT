using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gt.Core.Api
{
	public class ApiResult
	{
		public string StateCode { get; set; }
		public string Error { get; set; }
		public dynamic Data { get; set; }
	}
}
