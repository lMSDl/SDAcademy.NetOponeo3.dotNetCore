using Microsoft.EntityFrameworkCore.Migrations;

namespace Services.MsSqlService.Migrations
{
    public partial class ChangeLoginIndexName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_User_Login",
                table: "User",
                newName: "Index_Login");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "Index_Login",
                table: "User",
                newName: "IX_User_Login");
        }
    }
}
