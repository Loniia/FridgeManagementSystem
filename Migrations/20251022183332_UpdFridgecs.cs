using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdFridgecs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FaultID",
                table: "Fridge",
                newName: "FaultReportId");

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 22), new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2014), 5211m, 2, new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(1798) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 22), new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2037), "Double Door", 7374m, 4, new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2020) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 22), new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2045), 10706m, 2, new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2038) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 22), new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2052), "Mini Fridge", 7100m, 1, new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2046) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 22), new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2059), 8423m, 1, new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2053) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 22), new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2071), 11819m, 5, new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2065) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 22), new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2079), "Single Door", 10967m, 1, new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2073) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 22), new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2086), "Double Door", 10908m, 9, new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2080) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 22), new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2093), "Mini Fridge", 3833m, 9, new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2087) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 22), new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2136), 4191m, 6, new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2114) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 22), new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2165), "Single Door", 7741m, 7, new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2157) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 22), new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2178), "Single Door", 10441m, 3, new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2166) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FaultReportId",
                table: "Fridge",
                newName: "FaultID");

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 21), new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6774), 6743m, 6, new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6667) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 21), new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6792), "Mini Fridge", 6381m, 6, new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6779) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 21), new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6805), 5185m, 3, new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6794) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 21), new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6816), "Double Door", 8884m, 9, new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6806) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 21), new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6826), 10865m, 6, new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6817) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 21), new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6939), 4250m, 6, new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6928) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 21), new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6951), "Mini Fridge", 5852m, 9, new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6941) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 21), new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6962), "Single Door", 4663m, 1, new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6952) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 21), new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6972), "Single Door", 11209m, 4, new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6963) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 21), new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(7020), 9622m, 7, new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(6991) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 21), new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(7622), "Double Door", 10292m, 3, new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(7606) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 21), new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(7638), "Double Door", 6696m, 8, new DateTime(2025, 10, 21, 12, 36, 22, 85, DateTimeKind.Local).AddTicks(7623) });
        }
    }
}
