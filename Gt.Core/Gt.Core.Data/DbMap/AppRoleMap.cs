using Gt.Core.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gt.Core.Data.DbMap
{
	public class AppRoleMap : IEntityTypeConfiguration<AppRole>
	{
		public void Configure(EntityTypeBuilder<AppRole> builder)
		{
			builder.Property(t => t.RoleName).HasMaxLength(50).IsRequired();
			builder.Property(t => t.Description).HasMaxLength(50);

			//Initial data
			builder.HasData(new AppRole() { Id = 1, RoleName = "Admin", Description = "Administrator" });
			builder.HasData(new AppRole() { Id = 2, RoleName = "Customer", Description = "Customer" });
		}
	}
}
