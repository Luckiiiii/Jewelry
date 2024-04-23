using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jewelry.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameProduct",
                table: "Warranties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameProduct",
                table: "Warranties");
        }
    }
}
