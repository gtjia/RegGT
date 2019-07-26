using Gt.Core.Data;
using Gt.Core.Model.Entities;
using Gt.Core.Model.Models;
using Gt.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Gt.Core.Service
{
	public class AuthorizationService: IService
	{
		private readonly IConfiguration _configuration;
		private GtDbContext _db;
		public AuthorizationService(IConfiguration configuration, GtDbContext db)
		{
			_configuration = configuration;
			_db = db;
		}

		public UserModel Login(LoginUserModel loginUser)
		{
			var user = _db.AppUsers.Include(u => u.Role).FirstOrDefault(u => u.UserName == loginUser.UserName);
			if (user == null)
				throw new BussinessException("用户不存在！");
			if (user.Password != loginUser.Password)
				throw new BussinessException("输入密码错误！");

			var model = new UserModel()
			{
				Id = user.Id,
				UserName = user.UserName,
				RealName = user.RealName,
				PictureUrl = user.PictureUrl,
				RoleId = user.RoleId,
				RoleName = user.Role.RoleName,
				scope = new string[] { "test", "admin" }
			};
			return model;
		}

		
	}
}
