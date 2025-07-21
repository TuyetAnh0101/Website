using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsStore.Migrations
{
    /// <inheritdoc />
    public partial class AddUserToTutorBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TutorBookings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TutorBookings_UserId",
                table: "TutorBookings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TutorBookings_AspNetUsers_UserId",
                table: "TutorBookings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TutorBookings_AspNetUsers_UserId",
                table: "TutorBookings");

            migrationBuilder.DropIndex(
                name: "IX_TutorBookings_UserId",
                table: "TutorBookings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TutorBookings");
        }
    }
}
