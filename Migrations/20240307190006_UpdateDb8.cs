using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jewelry.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Material_ProductItems_ProductItemId",
                table: "Material");

            migrationBuilder.DropForeignKey(
                name: "FK_Size_ProductItems_ProductItemId",
                table: "Size");

            migrationBuilder.DropIndex(
                name: "IX_Size_ProductItemId",
                table: "Size");

            migrationBuilder.DropIndex(
                name: "IX_Material_ProductItemId",
                table: "Material");

            migrationBuilder.DropColumn(
                name: "ProductItemId",
                table: "Size");

            migrationBuilder.DropColumn(
                name: "ProductItemId",
                table: "Material");

            migrationBuilder.AddColumn<int>(
                name: "MateriasId",
                table: "ProductItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PriceId",
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

            migrationBuilder.AddColumn<int>(
                name: "SizesId",
                table: "ProductItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PurchasePrice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasePrice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalesPrice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesPrice", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductItems_MateriasId",
                table: "ProductItems",
                column: "MateriasId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductItems_PriceId",
                table: "ProductItems",
                column: "PriceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductItems_SalesPriceId",
                table: "ProductItems",
                column: "SalesPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductItems_SizesId",
                table: "ProductItems",
                column: "SizesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductItems_Material_MateriasId",
                table: "ProductItems",
                column: "MateriasId",
                principalTable: "Material",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductItems_PurchasePrice_PriceId",
                table: "ProductItems",
                column: "PriceId",
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

            migrationBuilder.AddForeignKey(
                name: "FK_ProductItems_Size_SizesId",
                table: "ProductItems",
                column: "SizesId",
                principalTable: "Size",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductItems_Material_MateriasId",
                table: "ProductItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductItems_PurchasePrice_PriceId",
                table: "ProductItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductItems_SalesPrice_SalesPriceId",
                table: "ProductItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductItems_Size_SizesId",
                table: "ProductItems");

            migrationBuilder.DropTable(
                name: "PurchasePrice");

            migrationBuilder.DropTable(
                name: "SalesPrice");

            migrationBuilder.DropIndex(
                name: "IX_ProductItems_MateriasId",
                table: "ProductItems");

            migrationBuilder.DropIndex(
                name: "IX_ProductItems_PriceId",
                table: "ProductItems");

            migrationBuilder.DropIndex(
                name: "IX_ProductItems_SalesPriceId",
                table: "ProductItems");

            migrationBuilder.DropIndex(
                name: "IX_ProductItems_SizesId",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "MateriasId",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "PriceId",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "SalesPriceId",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "SizesId",
                table: "ProductItems");

            migrationBuilder.AddColumn<int>(
                name: "ProductItemId",
                table: "Size",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductItemId",
                table: "Material",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Size_ProductItemId",
                table: "Size",
                column: "ProductItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Material_ProductItemId",
                table: "Material",
                column: "ProductItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Material_ProductItems_ProductItemId",
                table: "Material",
                column: "ProductItemId",
                principalTable: "ProductItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Size_ProductItems_ProductItemId",
                table: "Size",
                column: "ProductItemId",
                principalTable: "ProductItems",
                principalColumn: "Id");
        }
    }
}
