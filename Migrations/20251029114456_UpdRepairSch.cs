using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdRepairSch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairSchedules_Employees_Employee",
                table: "RepairSchedules");

            migrationBuilder.DropIndex(
                name: "IX_RepairSchedules_Employee",
                table: "RepairSchedules");

            migrationBuilder.DropColumn(
                name: "Employee",
                table: "RepairSchedules");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Employee",
                table: "RepairSchedules",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RepairSchedules_Employee",
                table: "RepairSchedules",
                column: "Employee");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairSchedules_Employees_Employee",
                table: "RepairSchedules",
                column: "Employee",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }
    }
}
