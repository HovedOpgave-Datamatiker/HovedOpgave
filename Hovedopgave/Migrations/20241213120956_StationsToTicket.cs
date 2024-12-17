using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hovedopgave.Migrations
{
    /// <inheritdoc />
    public partial class StationsToTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StationId",
                table: "Ticket",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Ticket",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "LastUpdated", "StationId" },
                values: new object[] { new DateTime(2024, 12, 13, 13, 9, 55, 700, DateTimeKind.Local).AddTicks(4606), new DateTime(2024, 12, 13, 13, 9, 55, 700, DateTimeKind.Local).AddTicks(4641), null });

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_StationId",
                table: "Ticket",
                column: "StationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Station_StationId",
                table: "Ticket",
                column: "StationId",
                principalTable: "Station",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Station_StationId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_StationId",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "StationId",
                table: "Ticket");

            migrationBuilder.UpdateData(
                table: "Ticket",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "LastUpdated" },
                values: new object[] { new DateTime(2024, 12, 10, 11, 49, 24, 581, DateTimeKind.Local).AddTicks(6360), new DateTime(2024, 12, 10, 11, 49, 24, 581, DateTimeKind.Local).AddTicks(6431) });
        }
    }
}
