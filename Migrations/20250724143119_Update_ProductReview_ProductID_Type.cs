using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsStore.Migrations
{
    /// <inheritdoc />
    public partial class Update_ProductReview_ProductID_Type : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Products_ProductID1",
                table: "ProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_ProductReviews_ProductID1",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "ProductID1",
                table: "ProductReviews");

            migrationBuilder.AlterColumn<long>(
                name: "ProductID",
                table: "ProductReviews",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_ProductID",
                table: "ProductReviews",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Products_ProductID",
                table: "ProductReviews",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Products_ProductID",
                table: "ProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_ProductReviews_ProductID",
                table: "ProductReviews");

            migrationBuilder.AlterColumn<int>(
                name: "ProductID",
                table: "ProductReviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "ProductID1",
                table: "ProductReviews",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_ProductID1",
                table: "ProductReviews",
                column: "ProductID1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Products_ProductID1",
                table: "ProductReviews",
                column: "ProductID1",
                principalTable: "Products",
                principalColumn: "ProductID");
        }
    }
}
