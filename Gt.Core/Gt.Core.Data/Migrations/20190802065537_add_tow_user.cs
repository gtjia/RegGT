using Microsoft.EntityFrameworkCore.Migrations;

namespace Gt.Core.Data.Migrations
{
    public partial class add_tow_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_AppRoles_RoleId",
                table: "AppUsers");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "AppUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "Description", "RoleName" },
                values: new object[] { 2, "Customer", "Customer" });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "Password", "PictureUrl", "RealName", "RoleId", "UserName" },
                values: new object[] { 2, "111111", null, "Guang Tao 1", 2, "User1" });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "Password", "PictureUrl", "RealName", "RoleId", "UserName" },
                values: new object[] { 3, "111111", null, "Guang Tao 2", 2, "User2" });

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_AppRoles_RoleId",
                table: "AppUsers",
                column: "RoleId",
                principalTable: "AppRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_AppRoles_RoleId",
                table: "AppUsers");

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "AppUsers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_AppRoles_RoleId",
                table: "AppUsers",
                column: "RoleId",
                principalTable: "AppRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
