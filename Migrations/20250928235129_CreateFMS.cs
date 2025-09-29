using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class CreateFMS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Fridges",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FaultID",
                table: "Fridges",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FaultID",
                table: "FaultReport",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FaultTechniciansTechnicianID",
                table: "FaultReport",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FridgeId1",
                table: "FaultReport",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusFilter",
                table: "FaultReport",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Faults",
                columns: table => new
                {
                    FaultID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FaultDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplianceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CustomerID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FridgeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faults", x => x.FaultID);
                    table.ForeignKey(
                        name: "FK_Faults_Fridges_FridgeId",
                        column: x => x.FridgeId,
                        principalTable: "Fridges",
                        principalColumn: "FridgeId");
                });

            migrationBuilder.CreateTable(
                name: "FaultTechnicians",
                columns: table => new
                {
                    TechnicianID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepairID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaultTechnicians", x => x.TechnicianID);
                });

            migrationBuilder.CreateTable(
                name: "RepairSchedules",
                columns: table => new
                {
                    RepairID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Diagnosis = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RepairDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ScheduleTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProgressNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RepairNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RepairCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PartsUsed = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FaultID = table.Column<int>(type: "int", nullable: false),
                    TechnicianID = table.Column<int>(type: "int", nullable: false),
                    FridgeID = table.Column<int>(type: "int", nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    ReportID = table.Column<int>(type: "int", nullable: false),
                    FaultReportsFaultReportId = table.Column<int>(type: "int", nullable: true),
                    FaultID1 = table.Column<int>(type: "int", nullable: true),
                    RepairScheduleRepairID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairSchedules", x => x.RepairID);
                    table.ForeignKey(
                        name: "FK_RepairSchedules_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK_RepairSchedules_FaultReport_FaultReportsFaultReportId",
                        column: x => x.FaultReportsFaultReportId,
                        principalTable: "FaultReport",
                        principalColumn: "FaultReportId");
                    table.ForeignKey(
                        name: "FK_RepairSchedules_FaultTechnicians_TechnicianID",
                        column: x => x.TechnicianID,
                        principalTable: "FaultTechnicians",
                        principalColumn: "TechnicianID");
                    table.ForeignKey(
                        name: "FK_RepairSchedules_Faults_FaultID",
                        column: x => x.FaultID,
                        principalTable: "Faults",
                        principalColumn: "FaultID");
                    table.ForeignKey(
                        name: "FK_RepairSchedules_Faults_FaultID1",
                        column: x => x.FaultID1,
                        principalTable: "Faults",
                        principalColumn: "FaultID");
                    table.ForeignKey(
                        name: "FK_RepairSchedules_Fridges_FridgeID",
                        column: x => x.FridgeID,
                        principalTable: "Fridges",
                        principalColumn: "FridgeId");
                    table.ForeignKey(
                        name: "FK_RepairSchedules_RepairSchedules_RepairScheduleRepairID",
                        column: x => x.RepairScheduleRepairID,
                        principalTable: "RepairSchedules",
                        principalColumn: "RepairID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fridges_SerialNumber",
                table: "Fridges",
                column: "SerialNumber",
                unique: true,
                filter: "[SerialNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FaultReport_FaultID",
                table: "FaultReport",
                column: "FaultID");

            migrationBuilder.CreateIndex(
                name: "IX_FaultReport_FaultTechniciansTechnicianID",
                table: "FaultReport",
                column: "FaultTechniciansTechnicianID");

            migrationBuilder.CreateIndex(
                name: "IX_FaultReport_FridgeId1",
                table: "FaultReport",
                column: "FridgeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Faults_FridgeId",
                table: "Faults",
                column: "FridgeId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairSchedules_CustomerID",
                table: "RepairSchedules",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_RepairSchedules_FaultID",
                table: "RepairSchedules",
                column: "FaultID");

            migrationBuilder.CreateIndex(
                name: "IX_RepairSchedules_FaultID1",
                table: "RepairSchedules",
                column: "FaultID1");

            migrationBuilder.CreateIndex(
                name: "IX_RepairSchedules_FaultReportsFaultReportId",
                table: "RepairSchedules",
                column: "FaultReportsFaultReportId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairSchedules_FridgeID",
                table: "RepairSchedules",
                column: "FridgeID");

            migrationBuilder.CreateIndex(
                name: "IX_RepairSchedules_RepairScheduleRepairID",
                table: "RepairSchedules",
                column: "RepairScheduleRepairID");

            migrationBuilder.CreateIndex(
                name: "IX_RepairSchedules_TechnicianID",
                table: "RepairSchedules",
                column: "TechnicianID");

            migrationBuilder.AddForeignKey(
                name: "FK_FaultReport_FaultTechnicians_FaultTechniciansTechnicianID",
                table: "FaultReport",
                column: "FaultTechniciansTechnicianID",
                principalTable: "FaultTechnicians",
                principalColumn: "TechnicianID");

            migrationBuilder.AddForeignKey(
                name: "FK_FaultReport_Faults_FaultID",
                table: "FaultReport",
                column: "FaultID",
                principalTable: "Faults",
                principalColumn: "FaultID");

            migrationBuilder.AddForeignKey(
                name: "FK_FaultReport_Fridges_FridgeId1",
                table: "FaultReport",
                column: "FridgeId1",
                principalTable: "Fridges",
                principalColumn: "FridgeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FaultReport_FaultTechnicians_FaultTechniciansTechnicianID",
                table: "FaultReport");

            migrationBuilder.DropForeignKey(
                name: "FK_FaultReport_Faults_FaultID",
                table: "FaultReport");

            migrationBuilder.DropForeignKey(
                name: "FK_FaultReport_Fridges_FridgeId1",
                table: "FaultReport");

            migrationBuilder.DropTable(
                name: "RepairSchedules");

            migrationBuilder.DropTable(
                name: "FaultTechnicians");

            migrationBuilder.DropTable(
                name: "Faults");

            migrationBuilder.DropIndex(
                name: "IX_Fridges_SerialNumber",
                table: "Fridges");

            migrationBuilder.DropIndex(
                name: "IX_FaultReport_FaultID",
                table: "FaultReport");

            migrationBuilder.DropIndex(
                name: "IX_FaultReport_FaultTechniciansTechnicianID",
                table: "FaultReport");

            migrationBuilder.DropIndex(
                name: "IX_FaultReport_FridgeId1",
                table: "FaultReport");

            migrationBuilder.DropColumn(
                name: "FaultID",
                table: "Fridges");

            migrationBuilder.DropColumn(
                name: "FaultID",
                table: "FaultReport");

            migrationBuilder.DropColumn(
                name: "FaultTechniciansTechnicianID",
                table: "FaultReport");

            migrationBuilder.DropColumn(
                name: "FridgeId1",
                table: "FaultReport");

            migrationBuilder.DropColumn(
                name: "StatusFilter",
                table: "FaultReport");

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Fridges",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
