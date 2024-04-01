using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jewelry.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancellationDate",
                table: "Status");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Status",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Purity",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "OrderItem");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Status",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CancellationDate",
                table: "Status",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
