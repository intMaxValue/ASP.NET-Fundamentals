using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Homies.Data.Migrations
{
    public partial class MappingTableChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventsParticipants_AspNetUsers_HelperId",
                table: "EventsParticipants");

            migrationBuilder.AddForeignKey(
                name: "FK_EventsParticipants_AspNetUsers_HelperId",
                table: "EventsParticipants",
                column: "HelperId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventsParticipants_AspNetUsers_HelperId",
                table: "EventsParticipants");

            migrationBuilder.AddForeignKey(
                name: "FK_EventsParticipants_AspNetUsers_HelperId",
                table: "EventsParticipants",
                column: "HelperId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
