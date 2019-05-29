using Microsoft.EntityFrameworkCore.Migrations;

namespace WTP.DAL.Migrations
{
    public partial class TeamInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Team_AspNetUsers_AppUserId",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Team_AppUserId",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Team");

            migrationBuilder.AddColumn<int>(
                name: "CoachId",
                table: "Team",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "Team",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GoalId",
                table: "Team",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Team",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServerId",
                table: "Team",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WinRate",
                table: "Team",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Team_CoachId",
                table: "Team",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_GameId",
                table: "Team",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_GoalId",
                table: "Team",
                column: "GoalId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_ServerId",
                table: "Team",
                column: "ServerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Team_AspNetUsers_CoachId",
                table: "Team",
                column: "CoachId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Games_GameId",
                table: "Team",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Goals_GoalId",
                table: "Team",
                column: "GoalId",
                principalTable: "Goals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Servers_ServerId",
                table: "Team",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Team_AspNetUsers_CoachId",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_Games_GameId",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_Goals_GoalId",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_Servers_ServerId",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Team_CoachId",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Team_GameId",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Team_GoalId",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Team_ServerId",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "GoalId",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "ServerId",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "WinRate",
                table: "Team");

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Team",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Team_AppUserId",
                table: "Team",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Team_AspNetUsers_AppUserId",
                table: "Team",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
