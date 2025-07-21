using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsStore.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeliveredToRental : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BookTitle",
                table: "Rentals",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsDelivered",
                table: "Rentals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "Rentals",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_ProductId",
                table: "Rentals",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Products_ProductId",
                table: "Rentals",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Products_ProductId",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_Rentals_ProductId",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "IsDelivered",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Rentals");

            migrationBuilder.AlterColumn<string>(
                name: "BookTitle",
                table: "Rentals",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);
        }
    }
}
