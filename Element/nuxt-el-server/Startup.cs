using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace nuxt_el_server
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
						ValidAudience = "yourdomain.com",//Audience
						ValidIssuer = "yourdomain.com",//Issuer，这两项和前面签发jwt的设置一致
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecurityKey"]))//拿到SecurityKey
					};
				});

			services.AddCors(options =>
			 options.AddPolicy("any",
				builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().AllowCredentials())
			 )
			 .AddCors(options => options.AddPolicy("AllowSameDomain",
				builder => builder.WithOrigins(new string[] { "http://localhost:80", "http://localhost:8080", "http://localhost:3000" }).AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().AllowCredentials())
			 );

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
	}
}
