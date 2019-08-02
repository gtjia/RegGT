using Gt.Core.Model.Entities;
using Gt.Core.Model.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Gt.Core.Api.Jwt
{
	public class TokenDto
	{
		private JwtSettings _jwtSettings;

		public string Token { get; set; }
		public UserModel User { get; set; }
		/// <summary>
		/// 生成token的时间
		/// </summary>
		public DateTime TokenTime { get; set; }
		public DateTime LastAccess { get; set; }

		public TokenDto(JwtSettings jwtSettings)
		{
			_jwtSettings = jwtSettings;
		}

		public string GenerateToken()
		{
			var claim = new Claim[]{
					new Claim(ClaimTypes.Name, this.User.UserName),
					new Claim(ClaimTypes.Role, this.User.RoleName),
					//new Claim("UserInfo", json)
				};

			//对称秘钥
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
			//签名证书(秘钥，加密算法)
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			//生成token
			var authTime = this.LastAccess;
			var expiresAt = authTime.AddMinutes(_jwtSettings.ExpireMinutes);

			var jwtToken = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claim, authTime, expiresAt, creds);
			this.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
			this.TokenTime = authTime;

			return this.Token;
		}

		public bool IsTokenExpired
		{
			get
			{
				return this.TokenTime.AddMinutes(_jwtSettings.ExpireMinutes) < DateTime.Now;
			}
		}
		public bool CanRefreshToken
		{
			get
			{
				return this.LastAccess.AddMinutes(_jwtSettings.ExpireMinutes) > DateTime.Now;
			}
		}
	}
}
