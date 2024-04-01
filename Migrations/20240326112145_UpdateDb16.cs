using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jewelry.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Status_AspNetUsers_UserId",
                table: "Status");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Status",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Status",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Status_AspNetUsers_UserId",
                table: "Status",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Status_AspNetUsers_UserId",
                table: "Status");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Status");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Status",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Status_AspNetUsers_UserId",
                table: "Status",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
