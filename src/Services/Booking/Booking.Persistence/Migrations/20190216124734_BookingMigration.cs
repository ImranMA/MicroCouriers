using Microsoft.EntityFrameworkCore.Migrations;

namespace Booking.Persistence.Migrations
{
    public partial class BookingMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingOrderId = table.Column<string>(nullable: false),
                    CustomerID = table.Column<string>(nullable: true),
                    PaymentID = table.Column<string>(nullable: true),
                    NotificationID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingOrderId);
                });

            migrationBuilder.CreateTable(
                name: "BookingsDetails",
                columns: table => new
                {
                    BookingOrderDetailId = table.Column<string>(nullable: false),
                    BookingOrderId = table.Column<string>(nullable: true),
                    PackageType = table.Column<string>(nullable: true),
                    Origin = table.Column<string>(nullable: true),
                    Destination = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingsDetails", x => x.BookingOrderDetailId);
                    table.ForeignKey(
                        name: "FK_BookingsDetails_Bookings_BookingOrderId",
                        column: x => x.BookingOrderId,
                        principalTable: "Bookings",
                        principalColumn: "BookingOrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingsDetails_BookingOrderId",
                table: "BookingsDetails",
                column: "BookingOrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingsDetails");

            migrationBuilder.DropTable(
                name: "Bookings");
        }
    }
}
