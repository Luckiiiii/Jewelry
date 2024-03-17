using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jewelry.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductItems_PurchasePrice_PurchasePriceId",
                table: "ProductItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductItems_SalesPrice_SalesPriceId",
                table: "ProductItems");

            migrationBuilder.DropIndex(
                name: "IX_ProductItems_PurchasePriceId",
                table: "ProductItems");

            migrationBuilder.DropIndex(
                name: "IX_ProductItems_SalesPriceId",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "PurchasePriceId",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "SalesPriceId",
                table: "ProductItems");

            migrationBuilder.AddColumn<int>(
                name: "ProductItemId",
                table: "SalesPrice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductItemId",
                table: "PurchasePrice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SalesPrice_ProductItemId",
                table: "SalesPrice",
                column: "ProductItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasePrice_ProductItemId",
                table: "PurchasePrice",
                column: "ProductItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasePrice_ProductItems_ProductItemId",
                table: "PurchasePrice",
                column: "ProductItemId",
                principalTable: "ProductItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPrice_ProductItems_ProductItemId",
                table: "SalesPrice",
                column: "ProductItemId",
                principalTable: "ProductItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasePrice_ProductItems_ProductItemId",
                table: "PurchasePrice");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesPrice_ProductItems_ProductItemId",
                table: "SalesPrice");

            migrationBuilder.DropIndex(
                name: "IX_SalesPrice_ProductItemId",
                table: "SalesPrice");

            migrationBuilder.DropIndex(
                name: "IX_PurchasePrice_ProductItemId",
                table: "PurchasePrice");

            migrationBuilder.DropColumn(
                name: "ProductItemId",
                table: "SalesPrice");

            migrationBuilder.DropColumn(
                name: "ProductItemId",
                table: "PurchasePrice");

            migrationBuilder.AddColumn<int>(
                name: "PurchasePriceId",
                table: "ProductItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SalesPriceId",
                table: "ProductItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductItems_PurchasePriceId",
                table: "ProductItems",
                column: "PurchasePriceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductItems_SalesPriceId",
                table: "ProductItems",
                column: "SalesPriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductItems_PurchasePrice_PurchasePriceId",
                table: "ProductItems",
                column: "PurchasePriceId",
                principalTable: "PurchasePrice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductItems_SalesPrice_SalesPriceId",
                table: "ProductItems",
                column: "SalesPriceId",
                principalTable: "SalesPrice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
