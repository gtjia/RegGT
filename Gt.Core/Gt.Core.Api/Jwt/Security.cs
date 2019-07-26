using Gt.Core.Model.Entities;
using Gt.Core.Model.Models;
using Gt.Core.Utilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Gt.Core.Api.Jwt
{
	public class Security
	{
		/// <summary>
		/// 已授权的 Token 信息集合
		/// </summary>
		private static ISet<TokenDto> _tokens = new HashSet<TokenDto>();

		private JwtSettings _jwtSettings;
		public Security(IOptions<JwtSettings> jwtSettingsAccesser)
		{
			_jwtSettings = jwtSettingsAccesser.Value;
		}

		public string CreateToken(UserModel user)
		{
			//string json = JsonConvert.SerializeObject(user, new JsonSerializerSettings()
			//{
			//	ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
			//});
			var claim = new Claim[]{
					new Claim(ClaimTypes.Name, user.UserName),
					//new Claim(ClaimTypes.Role, user.Role.RoleName),
					//new Claim("UserInfo", json)
				};

			//对称秘钥
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
			//签名证书(秘钥，加密算法)
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			//生成token
			var authTime = DateTime.Now;
			var expiresAt = authTime.AddMinutes(1);

			var jwtToken = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claim, authTime, expiresAt, creds);
			var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

			//缓存Token
			var dto = new TokenDto()
			{
				Token = token,
				User = user,
				LastAccess = authTime
			};
			_tokens.Add(dto);

			return token;
		}

		/// <summary>
		/// 判断是否存在当前 Token
		/// </summary>
		/// <param name="token">Token</param>
		/// <returns></returns>
		public TokenDto GetExistenceToken(string token)
		{
			if (string.IsNullOrWhiteSpace(token))
				return null;
			if (token.StartsWith("Bearer "))
				token = token.Substring("Bearer ".Length);
			var tokenDto = _tokens.SingleOrDefault(x => x.Token == token);
			return tokenDto;
		} 

		/// <summary>
		/// 移除Token
		/// </summary>
		/// <param name="token"></param>
		public void RemoveToken(string token)
			=> _tokens.Remove(GetExistenceToken(token));

		/// <summary>
		/// 刷新Token
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public string RefreshToken(string token)
		{
			var oldDto = GetExistenceToken(token);
			if (oldDto == null)
			{
				throw new BussinessException("未获取到当前 Token 信息");
			}

			_tokens.Remove(oldDto);
			var newToken = CreateToken(oldDto.User);
			return newToken;
		}

		//public UserModel GetUser(ClaimsPrincipal principal)
		//{
		//	string userInfo = principal.Claims.FirstOrDefault(p => p.Type == "UserInfo").Value;
		//	var user = JsonConvert.DeserializeObject<UserModel>(userInfo, new JsonSerializerSettings()
		//	{
		//		ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
		//	});
		//	user.scope = new string[] { "test", "admin" };
		//	return user;
		//}

		public UserModel GetUser(string token)
		{
			var dto = GetExistenceToken(token);
			if (dto != null)
				return dto.User;
			else
				return null;
		}
	}
}
