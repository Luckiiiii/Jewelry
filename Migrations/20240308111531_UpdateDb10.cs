using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jewelry.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductItems_Material_MateriasId",
                table: "ProductItems");

            migrationBuilder.RenameColumn(
                name: "MateriasId",
                table: "ProductItems",
                newName: "MaterialsId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductItems_MateriasId",
                table: "ProductItems",
                newName: "IX_ProductItems_MaterialsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductItems_Material_MaterialsId",
                table: "ProductItems",
                column: "MaterialsId",
                principalTable: "Material",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductItems_Material_MaterialsId",
                table: "ProductItems");

            migrationBuilder.RenameColumn(
                name: "MaterialsId",
                table: "ProductItems",
                newName: "MateriasId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductItems_MaterialsId",
                table: "ProductItems",
                newName: "IX_ProductItems_MateriasId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductItems_Material_MateriasId",
                table: "ProductItems",
                column: "MateriasId",
                principalTable: "Material",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
