# Gt.Common.Api
用JWT作认证的Webapi

# 1. 签发JWT token 认证
    [AllowAnonymous]
		// POST api/auth/login
		[HttpPost("login")]
		public IActionResult Login([FromBody]TokenRequest request)
		{
			if (request.Username == "AngelaDaddy" && request.Password == "123456")
			{
				// push the user’s name into a claim, so we can identify the user later on.
				var claims = new[]
				{
				   new Claim(ClaimTypes.Name, request.Username)
			   };
				//sign the token using a secret key.This secret will be shared between your API and anything that needs to check that the token is legit.
				var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
				var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
				//.NET Core’s JwtSecurityToken class takes on the heavy lifting and actually creates the token.
				/**
				 * Claims (Payload)
					Claims 部分包含了一些跟这个 token 有关的重要信息。 JWT 标准规定了一些字段，下面节选一些字段:

					iss: The issuer of the token，token 是给谁的
					sub: The subject of the token，token 主题
					exp: Expiration Time。 token 过期时间，Unix 时间戳格式
					iat: Issued At。 token 创建时间， Unix 时间戳格式
					jti: JWT ID。针对当前 token 的唯一标识
					除了规定的字段外，可以包含其他任何 JSON 兼容的字段。
				 * */
				var token = new JwtSecurityToken(
					issuer: "Gt.Common.Com",
					audience: "Gt.Common.Com",
					claims: claims,
					expires: DateTime.Now.AddMinutes(3),
					signingCredentials: creds);

				return Ok(new
				{
					token = new JwtSecurityTokenHandler().WriteToken(token)
				});
			}

			return BadRequest("Could not verify username and password");
		}
    
密钥SecurityKey是随意配置的，不过要注意不能过短，否则会报出KeySize的错儿。

以下方法供客户端查询用户信息：

    [HttpGet("user")]
		public IActionResult GetUser()
		{
			string userName = this.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Name).Value;

			var result = new
			{
				user = new
				{
					UserName = userName,
					scope = new string[] { "test", "admin" }
				}
			};
			return new JsonResult(result);
		}

# 2. 在Setup.cs中配置

在ConfigureServices中配置如下

      //添加jwt验证：
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,//是否验证Issuer
						ValidateAudience = true,//是否验证Audience
						ValidateLifetime = true,//是否验证失效时间
						ValidateIssuerSigningKey = true,//是否验证SecurityKey
						ValidAudience = "Gt.Common.Com",//Audience
						ValidIssuer = "Gt.Common.Com",//Issuer，这两项和前面签发jwt的设置一致
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecurityKey"]))//拿到SecurityKey
					};
				});
        
在Configure中启用安全验证

app.UseAuthentication();
