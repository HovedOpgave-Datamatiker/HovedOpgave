using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hovedopgave.Migrations
{
    public partial class AddUserTicketRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add the UserId column as nullable
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Ticket",
                type: "int",
                nullable: true); // Make it nullable to avoid conflicts

            // Update the Created and LastUpdated fields (optional, based on your application logic)
            migrationBuilder.UpdateData(
                table: "Ticket",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "LastUpdated" },
                values: new object[] { DateTime.Now, DateTime.Now });

            // Create the foreign key relationship
            migrationBuilder.CreateIndex(
                name: "IX_Ticket_UserId",
                table: "Ticket",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_User_UserId",
                table: "Ticket",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull); // Set null on delete to avoid cascading issues
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the foreign key and index
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_User_UserId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_UserId",
                table: "Ticket");

            // Drop the UserId column
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Ticket");
        }
    }
}
