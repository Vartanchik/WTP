using Microsoft.EntityFrameworkCore.Migrations;

namespace WTP.DAL.Migrations
{
    public partial class AddConfigHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_History_AspNetUsers_AppUserId",
                table: "History");

            migrationBuilder.DropForeignKey(
                name: "FK_History_Operation_OperationId",
                table: "History");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Operation",
                table: "Operation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_History",
                table: "History");

            migrationBuilder.DropIndex(
                name: "IX_History_AppUserId",
                table: "History");

            migrationBuilder.RenameTable(
                name: "Operation",
                newName: "Operations");

            migrationBuilder.RenameTable(
                name: "History",
                newName: "Histories");

            migrationBuilder.RenameIndex(
                name: "IX_History_OperationId",
                table: "Histories",
                newName: "IX_Histories_OperationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Operations",
                table: "Operations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Histories",
                table: "Histories",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Operations",
                columns: new[] { "Id", "OperationName" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 4 },
                    { 5, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Histories_AdminId",
                table: "Histories",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_AspNetUsers_AdminId",
                table: "Histories",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_Operations_OperationId",
                table: "Histories",
                column: "OperationId",
                principalTable: "Operations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Histories_AspNetUsers_AdminId",
                table: "Histories");

            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Operations_OperationId",
                table: "Histories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Operations",
                table: "Operations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Histories",
                table: "Histories");

            migrationBuilder.DropIndex(
                name: "IX_Histories_AdminId",
                table: "Histories");

            migrationBuilder.DeleteData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.RenameTable(
                name: "Operations",
                newName: "Operation");

            migrationBuilder.RenameTable(
                name: "Histories",
                newName: "History");

            migrationBuilder.RenameIndex(
                name: "IX_Histories_OperationId",
                table: "History",
                newName: "IX_History_OperationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Operation",
                table: "Operation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_History",
                table: "History",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_History_AppUserId",
                table: "History",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_History_AspNetUsers_AppUserId",
                table: "History",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_History_Operation_OperationId",
                table: "History",
                column: "OperationId",
                principalTable: "Operation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
