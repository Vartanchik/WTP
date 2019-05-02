using Microsoft.EntityFrameworkCore.Migrations;

namespace WTP.DAL.Migrations
{
    public partial class AddUserDeletedStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DeletedStatus",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedStatus",
                table: "AspNetUsers");
        }
    }
}
