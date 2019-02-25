using Microsoft.EntityFrameworkCore.Migrations;

namespace Payment.Persistence.Migrations
{
    public partial class fixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "paymentStatus",
                table: "Payments",
                newName: "PaymentStatus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentStatus",
                table: "Payments",
                newName: "paymentStatus");
        }
    }
}
