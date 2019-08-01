using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gt.Core.Api.Jwt
{
	public class JwtSettings: IOptions<JwtSettings>
	{
		//token是谁颁发的
		public string Issuer { get; set; }
		//token可以给哪些客户端使用
		public string Audience { get; set; }
		//加密的key
		public string SecretKey { get; set; }
		/// <summary>
		/// 超时时间，单位为分钟
		/// </summary>
		public int ExpireMinutes { get; set; }

		public JwtSettings Value => this;
	}
}
