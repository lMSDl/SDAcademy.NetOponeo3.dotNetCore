using Microsoft.EntityFrameworkCore.Migrations;

namespace Services.MsSqlService.Migrations
{
    public partial class AddProducer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Producer",
                table: "Tire");

            migrationBuilder.AddColumn<int>(
                name: "ProducerId",
                table: "Tire",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Producer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producer", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tire_ProducerId",
                table: "Tire",
                column: "ProducerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tire_Producer_ProducerId",
                table: "Tire",
                column: "ProducerId",
                principalTable: "Producer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tire_Producer_ProducerId",
                table: "Tire");

            migrationBuilder.DropTable(
                name: "Producer");

            migrationBuilder.DropIndex(
                name: "IX_Tire_ProducerId",
                table: "Tire");

            migrationBuilder.DropColumn(
                name: "ProducerId",
                table: "Tire");

            migrationBuilder.AddColumn<string>(
                name: "Producer",
                table: "Tire",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
