﻿// <auto-generated />
using Gt.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Gt.Core.Data.Migrations
{
    [DbContext(typeof(GtDbContext))]
    partial class GtDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Gt.Core.Model.Entities.AppRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasMaxLength(50);

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("AppRoles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Administrator",
                            RoleName = "Admin"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Customer",
                            RoleName = "Customer"
                        });
                });

            modelBuilder.Entity("Gt.Core.Model.Entities.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("PictureUrl")
                        .HasMaxLength(500);

                    b.Property<string>("RealName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("RoleId");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AppUsers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Password = "111111",
                            RealName = "Guang Tao",
                            RoleId = 1,
                            UserName = "Admin"
                        },
                        new
                        {
                            Id = 2,
                            Password = "111111",
                            RealName = "Guang Tao 1",
                            RoleId = 2,
                            UserName = "User1"
                        },
                        new
                        {
                            Id = 3,
                            Password = "111111",
                            RealName = "Guang Tao 2",
                            RoleId = 2,
                            UserName = "User2"
                        });
                });

            modelBuilder.Entity("Gt.Core.Model.Entities.AppUser", b =>
                {
                    b.HasOne("Gt.Core.Model.Entities.AppRole", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
