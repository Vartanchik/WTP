using Microsoft.EntityFrameworkCore.Migrations;

namespace WTP.DAL.Migrations
{
    public partial class AddValueToRank : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Value",
                table: "Ranks",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 1,
                column: "Value",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 2,
                column: "Value",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 3,
                column: "Value",
                value: 30);

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 4,
                column: "Value",
                value: 40);

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 5,
                column: "Value",
                value: 50);

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 6,
                column: "Value",
                value: 60);

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 7,
                column: "Value",
                value: 70);

            migrationBuilder.UpdateData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8,
                column: "Value",
                value: 80);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "Ranks");
        }
    }
}
