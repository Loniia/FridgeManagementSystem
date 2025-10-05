using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UserLogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeID1",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmployeeID1",
                table: "AspNetUsers",
                column: "EmployeeID1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Employees_EmployeeID1",
                table: "AspNetUsers",
                column: "EmployeeID1",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Employees_EmployeeID1",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmployeeID1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmployeeID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmployeeID1",
                table: "AspNetUsers");
        }
    }
}
