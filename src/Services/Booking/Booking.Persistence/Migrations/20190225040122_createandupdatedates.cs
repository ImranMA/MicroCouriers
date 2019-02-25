using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Booking.Persistence.Migrations
{
    public partial class createandupdatedates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Bookings",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Bookings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Bookings");
        }
    }
}
