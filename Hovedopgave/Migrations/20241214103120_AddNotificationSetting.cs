using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hovedopgave.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationSetting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EmailNotificationsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    Frequency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationSetting_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Ticket",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "LastUpdated" },
                values: new object[] { new DateTime(2024, 12, 14, 11, 31, 20, 95, DateTimeKind.Local).AddTicks(8355), new DateTime(2024, 12, 14, 11, 31, 20, 95, DateTimeKind.Local).AddTicks(8409) });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationSetting_UserId",
                table: "NotificationSetting",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationSetting");

            migrationBuilder.UpdateData(
                table: "Ticket",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "LastUpdated" },
                values: new object[] { new DateTime(2024, 12, 10, 11, 49, 24, 581, DateTimeKind.Local).AddTicks(6360), new DateTime(2024, 12, 10, 11, 49, 24, 581, DateTimeKind.Local).AddTicks(6431) });
        }
    }
}
