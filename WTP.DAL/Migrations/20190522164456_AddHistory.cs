using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WTP.DAL.Migrations
{
    public partial class AddHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Operation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OperationName = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "History",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateOfOperation = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    PreviousUserEmail = table.Column<string>(nullable: true),
                    PreviousUserName = table.Column<string>(nullable: true),
                    NewUserEmail = table.Column<string>(nullable: true),
                    NewUserName = table.Column<string>(nullable: true),
                    AppUserId = table.Column<int>(nullable: false),
                    AdminId = table.Column<int>(nullable: false),
                    OperationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_History", x => x.Id);
                    table.ForeignKey(
                        name: "FK_History_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_History_Operation_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Operation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_History_AppUserId",
                table: "History",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_History_OperationId",
                table: "History",
                column: "OperationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "History");

            migrationBuilder.DropTable(
                name: "Operation");
        }
    }
}
