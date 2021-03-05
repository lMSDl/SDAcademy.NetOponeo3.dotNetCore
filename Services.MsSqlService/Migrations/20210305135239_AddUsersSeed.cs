using Microsoft.EntityFrameworkCore.Migrations;

namespace Services.MsSqlService.Migrations
{
    public partial class AddUsersSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Login", "Password", "Role" },
                values: new object[] { 1, "Admin", "nimdA", 31 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
