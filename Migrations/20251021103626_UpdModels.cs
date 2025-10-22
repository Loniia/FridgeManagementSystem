using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairSchedules_Faults_FaultID",
                table: "RepairSchedules");

            migrationBuilder.AddColumn<int>(
                name: "FaultID1",
                table: "RepairSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FaultReportId",
                table: "RepairSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Fridge",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Notes", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 21), new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6774), null, 6743m, 6, new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6667) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Notes", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 21), new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6792), "Mini Fridge", null, 6381m, 6, new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6779) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Notes", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 21), new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6805), "Single Door", null, 5185m, 3, new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6794) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Notes", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 21), new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6816), "Double Door", null, 8884m, 9, new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6806) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Notes", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 21), new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6826), "Double Door", null, 10865m, 6, new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6817) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Notes", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 21), new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6939), "Mini Fridge", null, 4250m, 6, new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6928) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Notes", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 21), new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6951), "Mini Fridge", null, 5852m, 9, new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6941) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Notes", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 21), new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6962), null, 4663m, 1, new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6952) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Notes", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 21), new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6972), "Single Door", null, 11209m, 4, new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6963) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Notes", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 21), new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(7020), "Single Door", null, 9622m, 7, new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6991) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Notes", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 21), new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(7622), "Double Door", null, 10292m, 3, new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(7606) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Notes", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 21), new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(7638), null, 6696m, 8, new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(7623) });

            migrationBuilder.CreateIndex(
                name: "IX_RepairSchedules_FaultID1",
                table: "RepairSchedules",
                column: "FaultID1");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairSchedules_FaultReport_FaultID",
                table: "RepairSchedules",
                column: "FaultID",
                principalTable: "FaultReport",
                principalColumn: "FaultReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairSchedules_Faults_FaultID1",
                table: "RepairSchedules",
                column: "FaultID1",
                principalTable: "Faults",
                principalColumn: "FaultID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairSchedules_FaultReport_FaultID",
                table: "RepairSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairSchedules_Faults_FaultID1",
                table: "RepairSchedules");

            migrationBuilder.DropIndex(
                name: "IX_RepairSchedules_FaultID1",
                table: "RepairSchedules");

            migrationBuilder.DropColumn(
                name: "FaultID1",
                table: "RepairSchedules");

            migrationBuilder.DropColumn(
                name: "FaultReportId",
                table: "RepairSchedules");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Fridge");

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 20), new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(7944), 6485m, 7, new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(7706) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 20), new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8010), "Double Door", 6153m, 9, new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(7948) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 20), new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8044), "Mini Fridge", 5855m, 4, new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8011) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 20), new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8068), "Mini Fridge", 5470m, 4, new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8045) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 20), new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8091), "Single Door", 3707m, 9, new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8070) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 20), new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8119), "Single Door", 8134m, 9, new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8098) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 20), new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8141), "Single Door", 5558m, 5, new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8120) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 20), new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8164), 11041m, 9, new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8143) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 20), new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8185), "Double Door", 7612m, 7, new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8165) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 20), new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8220), "Mini Fridge", 8227m, 9, new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8188) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 20), new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8270), "Mini Fridge", 8410m, 4, new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8248) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 20), new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8292), 5954m, 9, new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8272) });

            migrationBuilder.AddForeignKey(
                name: "FK_RepairSchedules_Faults_FaultID",
                table: "RepairSchedules",
                column: "FaultID",
                principalTable: "Faults",
                principalColumn: "FaultID");
        }
    }
}
