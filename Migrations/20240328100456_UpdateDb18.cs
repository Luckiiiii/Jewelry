using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jewelry.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Purity",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "OrderItem");

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "ProductItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Purity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductItemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purity_ProductItems_ProductItemId",
                        column: x => x.ProductItemId,
                        principalTable: "ProductItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Purity_ProductItemId",
                table: "Purity",
                column: "ProductItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Purity");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "ProductItems");

            migrationBuilder.AddColumn<string>(
                name: "Purity",
                table: "OrderItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "OrderItem",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
