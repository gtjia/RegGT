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

		public TokenDto CreateToken(UserModel user)
		{
			//缓存Token
			var dto = new TokenDto(_jwtSettings)
			{
				User = user,
				LastAccess = DateTime.Now
			};
			_tokens.Add(dto);

			return dto;
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

		public void RemoveToken(TokenDto token)
			=> _tokens.Remove(token);

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
