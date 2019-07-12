using Gt.Core.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gt.Core.Data.DbMap
{
	public class AppUserMap : IEntityTypeConfiguration<AppUser>
	{
		public void Configure(EntityTypeBuilder<AppUser> builder)
		{
			//builder.ToTable("Users");
			builder.Property(t => t.UserName).HasMaxLength(50).IsRequired();
			builder.Property(t => t.Password).HasMaxLength(50).IsRequired();
			builder.Property(t => t.RealName).HasMaxLength(50).IsRequired();
			builder.Property(t => t.PictureUrl).HasMaxLength(500);

			//这个可以不指定
			//builder.HasOne(r => r.Role).WithMany(u => u.Users).HasForeignKey("RoleId");

			//Initial data
			builder.HasData(new { Id = 1, UserName = "Admin", Password = "111111", RealName = "Guang Tao", RoleId= 1 });
		}
	}
}
