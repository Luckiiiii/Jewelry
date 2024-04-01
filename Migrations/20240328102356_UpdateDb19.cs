using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jewelry.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purity_ProductItems_ProductItemId",
                table: "Purity");

            migrationBuilder.DropIndex(
                name: "IX_Purity_ProductItemId",
                table: "Purity");

            migrationBuilder.DropColumn(
                name: "ProductItemId",
                table: "Purity");

            migrationBuilder.AddColumn<int>(
                name: "PurityId",
                table: "ProductItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductItems_PurityId",
                table: "ProductItems",
                column: "PurityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductItems_Purity_PurityId",
                table: "ProductItems",
                column: "PurityId",
                principalTable: "Purity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductItems_Purity_PurityId",
                table: "ProductItems");

            migrationBuilder.DropIndex(
                name: "IX_ProductItems_PurityId",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "PurityId",
                table: "ProductItems");

            migrationBuilder.AddColumn<int>(
                name: "ProductItemId",
                table: "Purity",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Purity_ProductItemId",
                table: "Purity",
                column: "ProductItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Purity_ProductItems_ProductItemId",
                table: "Purity",
                column: "ProductItemId",
                principalTable: "ProductItems",
                principalColumn: "Id");
        }
    }
}
