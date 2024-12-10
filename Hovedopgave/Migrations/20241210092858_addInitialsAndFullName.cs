using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hovedopgave.Migrations
{
    /// <inheritdoc />
    public partial class addInitialsAndFullName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Initials",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Ticket",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "LastUpdated" },
                values: new object[] { new DateTime(2024, 12, 10, 10, 28, 57, 914, DateTimeKind.Local).AddTicks(3932), new DateTime(2024, 12, 10, 10, 28, 57, 914, DateTimeKind.Local).AddTicks(3980) });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FullName", "Initials" },
                values: new object[] { "Admin Adminsen", "AA" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FullName", "Initials" },
                values: new object[] { "User Useren", "UU" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FullName", "Initials" },
                values: new object[] { "Felt Feltsen", "FF" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "FullName", "Initials" },
                values: new object[] { "Kontor Kontorsen", "KK" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Initials",
                table: "User");

            migrationBuilder.UpdateData(
                table: "Ticket",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "LastUpdated" },
                values: new object[] { new DateTime(2024, 12, 9, 12, 47, 39, 86, DateTimeKind.Local).AddTicks(7188), new DateTime(2024, 12, 9, 12, 47, 39, 86, DateTimeKind.Local).AddTicks(7255) });
        }
    }
}
