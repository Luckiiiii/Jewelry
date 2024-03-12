using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jewelry.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductItems_PurchasePrice_PriceId",
                table: "ProductItems");

            migrationBuilder.RenameColumn(
                name: "PriceId",
                table: "ProductItems",
                newName: "PurchasePriceId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductItems_PriceId",
                table: "ProductItems",
                newName: "IX_ProductItems_PurchasePriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductItems_PurchasePrice_PurchasePriceId",
                table: "ProductItems",
                column: "PurchasePriceId",
                principalTable: "PurchasePrice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductItems_PurchasePrice_PurchasePriceId",
                table: "ProductItems");

            migrationBuilder.RenameColumn(
                name: "PurchasePriceId",
                table: "ProductItems",
                newName: "PriceId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductItems_PurchasePriceId",
                table: "ProductItems",
                newName: "IX_ProductItems_PriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductItems_PurchasePrice_PriceId",
                table: "ProductItems",
                column: "PriceId",
                principalTable: "PurchasePrice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
