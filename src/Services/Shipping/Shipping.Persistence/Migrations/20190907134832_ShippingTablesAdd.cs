using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Shipping.Persistence.Migrations
{
    public partial class ShippingTablesAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shippings",
                columns: table => new
                {
                    ShippingsId = table.Column<string>(nullable: false),
                    BookingOrderId = table.Column<string>(nullable: true),
                    CustomerId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shippings", x => x.ShippingsId);
                });

            migrationBuilder.CreateTable(
                name: "ShippingsHistory",
                columns: table => new
                {
                    ShippingsHistoryId = table.Column<string>(nullable: false),
                    ShippingStatus = table.Column<int>(nullable: false),
                    ShippingsId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingsHistory", x => x.ShippingsHistoryId);
                    table.ForeignKey(
                        name: "FK_ShippingsHistory_Shippings_ShippingsId",
                        column: x => x.ShippingsId,
                        principalTable: "Shippings",
                        principalColumn: "ShippingsId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShippingsHistory_ShippingsId",
                table: "ShippingsHistory",
                column: "ShippingsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShippingsHistory");

            migrationBuilder.DropTable(
                name: "Shippings");
        }
    }
}
