using Microsoft.EntityFrameworkCore.Migrations;

namespace Services.MsSqlService.Migrations
{
    public partial class AddTireDimensionsIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tire_Width_Diameter_Profile",
                table: "Tire",
                columns: new[] { "Width", "Diameter", "Profile" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tire_Width_Diameter_Profile",
                table: "Tire");
        }
    }
}
