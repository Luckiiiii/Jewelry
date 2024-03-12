using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jewelry.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductPortfolio_PortfolioId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ProductPortfolio");

            migrationBuilder.DropColumn(
                name: "CancellationReason",
                table: "Status");

            migrationBuilder.DropColumn(
                name: "DeliveryImg",
                table: "Status");

            migrationBuilder.DropColumn(
                name: "RefundImg",
                table: "Status");

            migrationBuilder.DropColumn(
                name: "ArtId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "InventoryReceipt");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "InventoryReceipt");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "InventoryReceipt");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Products",
                newName: "WarrantyInformation");

            migrationBuilder.RenameColumn(
                name: "PortfolioId",
                table: "Products",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Products",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_Products_PortfolioId",
                table: "Products",
                newName: "IX_Products_CategoryId");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Orders",
                newName: "DeliveryAddress");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "InventoryReceipt",
                newName: "CreationDate");

            migrationBuilder.AddColumn<string>(
                name: "ConsigneeName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "InventoryReceiptDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Confirmation",
                table: "InventoryReceipt",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmationDate",
                table: "InventoryReceipt",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Material",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductItemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Material", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Material_ProductItems_ProductItemId",
                        column: x => x.ProductItemId,
                        principalTable: "ProductItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Material_ProductItemId",
                table: "Material",
                column: "ProductItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductCategory_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "ProductCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductCategory_CategoryId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Material");

            migrationBuilder.DropTable(
                name: "ProductCategory");

            migrationBuilder.DropColumn(
                name: "ConsigneeName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "InventoryReceiptDetails");

            migrationBuilder.DropColumn(
                name: "Confirmation",
                table: "InventoryReceipt");

            migrationBuilder.DropColumn(
                name: "ConfirmationDate",
                table: "InventoryReceipt");

            migrationBuilder.RenameColumn(
                name: "WarrantyInformation",
                table: "Products",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Products",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Products",
                newName: "PortfolioId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                newName: "IX_Products_PortfolioId");

            migrationBuilder.RenameColumn(
                name: "DeliveryAddress",
                table: "Orders",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "InventoryReceipt",
                newName: "CreatedDate");

            migrationBuilder.AddColumn<string>(
                name: "CancellationReason",
                table: "Status",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryImg",
                table: "Status",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RefundImg",
                table: "Status",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ArtId",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ProductItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "InventoryReceipt",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "InventoryReceipt",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "InventoryReceipt",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProductPortfolio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPortfolio", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductPortfolio_PortfolioId",
                table: "Products",
                column: "PortfolioId",
                principalTable: "ProductPortfolio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
