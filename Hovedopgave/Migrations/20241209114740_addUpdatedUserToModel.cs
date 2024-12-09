using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hovedopgave.Migrations
{
    /// <inheritdoc />
    public partial class addUpdatedUserToModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "Ticket",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Ticket",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "LastUpdated", "LastUpdatedBy" },
                values: new object[] { new DateTime(2024, 12, 9, 12, 47, 39, 86, DateTimeKind.Local).AddTicks(7188), new DateTime(2024, 12, 9, 12, 47, 39, 86, DateTimeKind.Local).AddTicks(7255), null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "Ticket");

            migrationBuilder.UpdateData(
                table: "Ticket",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "LastUpdated" },
                values: new object[] { new DateTime(2024, 12, 9, 12, 21, 40, 69, DateTimeKind.Local).AddTicks(6900), new DateTime(2024, 12, 9, 12, 21, 40, 69, DateTimeKind.Local).AddTicks(6989) });
        }
    }
}
