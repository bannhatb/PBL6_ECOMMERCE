using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website_Ecommerce.API.Migrations
{
    public partial class Inital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Booked",
                table: "VoucherProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sale",
                table: "VoucherProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Booked",
                table: "VoucherOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sale",
                table: "VoucherOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Booked",
                table: "ProductDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Saled",
                table: "ProductDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Booked",
                table: "VoucherProducts");

            migrationBuilder.DropColumn(
                name: "Sale",
                table: "VoucherProducts");

            migrationBuilder.DropColumn(
                name: "Booked",
                table: "VoucherOrders");

            migrationBuilder.DropColumn(
                name: "Sale",
                table: "VoucherOrders");

            migrationBuilder.DropColumn(
                name: "Booked",
                table: "ProductDetails");

            migrationBuilder.DropColumn(
                name: "Saled",
                table: "ProductDetails");
        }
    }
}
