using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsStore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRentalWithPenaltyAndDelivery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "BadDebtReported",
                table: "Rentals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LateReturnDays",
                table: "Rentals",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PenaltyFee",
                table: "Rentals",
                type: "decimal(8,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BadDebtReported",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "LateReturnDays",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "PenaltyFee",
                table: "Rentals");
        }
    }
}
