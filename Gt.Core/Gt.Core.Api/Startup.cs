using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Gt.Core.Api.Jwt;
using Gt.Core.Data;
using Gt.Core.Model.Models;
using Gt.Core.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Gt.Core.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// Add framework services.
			this.ConfigureDB(services);
			this.ConfigureService(services);
			this.ConfigureAuth(services);


			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app.UseAuthentication();//注意添加这一句，启用验证

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseCors("AllowSameDomain");

			app.UseHttpsRedirection();
			app.UseMvc();
		}


		#region Configurations
		private void ConfigureDB(IServiceCollection services)
		{
			services.AddDbContext<GtDbContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
					b => b.MigrationsAssembly("Gt.Core.Data")));
		}

		private void ConfigureAuth(IServiceCollection services)
		{
			//将appsettings.json中的JwtSettings部分文件读取到JwtSettings中，这是给其他地方用的
			services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));

			services.AddScoped(typeof(Security));

			//由于初始化的时候我们就需要用，所以使用Bind的方式读取配置
			//将配置绑定到JwtSettings实例中
			var jwtSettings = new JwtSettings();
			Configuration.Bind("JwtSettings", jwtSettings);

			services.AddAuthentication(options => {
				//认证middleware配置
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(o => {
				//主要是jwt  token参数设置
				o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
				{
					//Token颁发机构
					ValidIssuer = jwtSettings.Issuer,
					//颁发给谁
					ValidAudience = jwtSettings.Audience,
					//这里的key要进行加密，需要引用Microsoft.IdentityModel.Tokens
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
					ValidateIssuer = true,//是否验证Issuer
					ValidateAudience = true,//是否验证Audience
					ValidateLifetime = true,//是否验证失效时间
					ValidateIssuerSigningKey = true,//是否验证SecurityKey
					ClockSkew = TimeSpan.Zero
				};
				//o.Events = new JwtBearerEvents
				//{
				//	OnMessageReceived = context =>
				//	{
				//		var accessToken = context.HttpContext.Request.Query["access_token"];
				//		var path = context.HttpContext.Request.Path;
				//		if (!(string.IsNullOrWhiteSpace(accessToken))
				//			&& path.StartsWithSegments("/hubs/message"))
				//		{
				//			context.Token = accessToken;
				//		}
				//		return Task.CompletedTask;
				//	},
				//	OnAuthenticationFailed = context =>
				//	{
				//		//Token expired
				//		if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
				//		{
				//			context.Response.Headers.Add("Token-Expired", "true");
				//		}
				//		else if (context.Exception.GetType() == typeof(SecurityTokenInvalidLifetimeException))
				//		{
				//			context.Response.Headers.Add("Token-Expired", "true");
				//		}
				//		return Task.CompletedTask;
				//	}
				//};
			});

			services.AddCors(options =>
			 options.AddPolicy("any",
				builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().AllowCredentials())
			 )
			 .AddCors(options => options.AddPolicy("AllowSameDomain",
				builder => builder.WithOrigins(new string[] { "http://localhost:80", "http://localhost:8080", "http://localhost:3000" }).AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().AllowCredentials())
			 );
		}

		private void ConfigureService(IServiceCollection services)
		{
			Assembly ass = Assembly.GetAssembly(typeof(IService));
			Type[] types = ass.GetTypes().Where(t => t.IsClass).ToArray();
			foreach (var t in types)
				services.AddTransient(t);
		}
		#endregion
	}
}
