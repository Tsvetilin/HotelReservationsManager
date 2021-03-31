using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddRoomNumberAndClientAdult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_EmployeeData_EmployeeDataUserId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmployeeDataUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmployeeDataUserId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdult",
                table: "ClientData",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeData_AspNetUsers_UserId",
                table: "EmployeeData",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeData_AspNetUsers_UserId",
                table: "EmployeeData");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "IsAdult",
                table: "ClientData");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeDataUserId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmployeeDataUserId",
                table: "AspNetUsers",
                column: "EmployeeDataUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_EmployeeData_EmployeeDataUserId",
                table: "AspNetUsers",
                column: "EmployeeDataUserId",
                principalTable: "EmployeeData",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
