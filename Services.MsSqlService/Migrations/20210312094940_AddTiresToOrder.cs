using Microsoft.EntityFrameworkCore.Migrations;

namespace Services.MsSqlService.Migrations
{
    public partial class AddTiresToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderTire",
                columns: table => new
                {
                    OrdersId = table.Column<int>(type: "int", nullable: false),
                    TiresId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTire", x => new { x.OrdersId, x.TiresId });
                    table.ForeignKey(
                        name: "FK_OrderTire_Order_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderTire_Tire_TiresId",
                        column: x => x.TiresId,
                        principalTable: "Tire",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderTire_TiresId",
                table: "OrderTire",
                column: "TiresId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderTire");
        }
    }
}
