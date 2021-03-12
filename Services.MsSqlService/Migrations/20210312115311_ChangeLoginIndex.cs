using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Services.MsSqlService.Migrations
{
    public partial class ChangeLoginIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Index_Login",
                table: "User");

            migrationBuilder.CreateIndex(
                name: "Index_Login",
                table: "User",
                column: "Login",
                unique: true)
                .Annotation("SqlServer:Include", new[] { "Password" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Index_Login",
                table: "User");

            migrationBuilder.CreateIndex(
                name: "Index_Login",
                table: "User",
                column: "Login",
                unique: true);
        }
    }
}
