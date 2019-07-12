using Gt.Core.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Gt.Core.Data
{
	public class GtDbContext : DbContext
	{
		public GtDbContext(DbContextOptions<GtDbContext> options)
		: base(options)
		{ }

		public DbSet<AppUser> AppUsers { get; set; }
		public DbSet<AppRole> AppRoles { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			//Write Fluent API configurations here
			Assembly maps = Assembly.Load(new AssemblyName("Gt.Core.Data"));
			modelBuilder.ApplyConfigurationsFromAssembly(maps);
		}
	}
}
