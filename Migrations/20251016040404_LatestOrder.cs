using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class LatestOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1295), "Mini Fridge", 6329m, 4, new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1107) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1319), "Mini Fridge", 10786m, 9, new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1300) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1352), "Double Door", 10418m, 8, new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1320) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1369), "Double Door", 11127m, 2, new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1354) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1384), "Double Door", 5009m, 7, new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1370) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1403), "Mini Fridge", 9494m, 3, new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1389) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1417), "Mini Fridge", 5764m, 7, new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1404) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1432), "Double Door", 9995m, new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1418) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1447), "Mini Fridge", 11550m, 1, new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1433) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1472), 8689m, 1, new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1450) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1511), "Double Door", 10372m, 4, new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1496) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1544), "Double Door", 7888m, 7, new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1530) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 16, 4, 32, 53, 100, DateTimeKind.Local).AddTicks(6146), "Double Door", 3556m, 5, new DateTime(2025, 10, 16, 4, 32, 53, 100, DateTimeKind.Local).AddTicks(5683) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 16, 4, 32, 53, 100, DateTimeKind.Local).AddTicks(6197), "Single Door", 11583m, 2, new DateTime(2025, 10, 16, 4, 32, 53, 100, DateTimeKind.Local).AddTicks(6157) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 16, 4, 32, 53, 100, DateTimeKind.Local).AddTicks(6234), "Single Door", 5561m, 3, new DateTime(2025, 10, 16, 4, 32, 53, 100, DateTimeKind.Local).AddTicks(6199) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 16, 4, 32, 53, 100, DateTimeKind.Local).AddTicks(6259), "Single Door", 11762m, 6, new DateTime(2025, 10, 16, 4, 32, 53, 100, DateTimeKind.Local).AddTicks(6236) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 16, 4, 32, 53, 100, DateTimeKind.Local).AddTicks(6309), "Mini Fridge", 9096m, 9, new DateTime(2025, 10, 16, 4, 32, 53, 100, DateTimeKind.Local).AddTicks(6284) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 16, 4, 32, 53, 100, DateTimeKind.Local).AddTicks(6340), "Double Door", 11585m, 7, new DateTime(2025, 10, 16, 4, 32, 53, 100, DateTimeKind.Local).AddTicks(6318) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 16, 4, 32, 53, 100, DateTimeKind.Local).AddTicks(6364), "Double Door", 6752m, 4, new DateTime(2025, 10, 16, 4, 32, 53, 100, DateTimeKind.Local).AddTicks(6341) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 16, 4, 32, 53, 100, DateTimeKind.Local).AddTicks(6387), "Single Door", 9672m, new DateTime(2025, 10, 16, 4, 32, 53, 100, DateTimeKind.Local).AddTicks(6365) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 16, 4, 32, 53, 100, DateTimeKind.Local).AddTicks(6409), "Double Door", 11988m, 2, new DateTime(2025, 10, 16, 4, 32, 53, 100, DateTimeKind.Local).AddTicks(6388) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 16, 4, 32, 53, 100, DateTimeKind.Local).AddTicks(6444), 9930m, 6, new DateTime(2025, 10, 16, 4, 32, 53, 100, DateTimeKind.Local).AddTicks(6413) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 16, 4, 32, 53, 100, DateTimeKind.Local).AddTicks(6497), "Single Door", 11900m, 9, new DateTime(2025, 10, 16, 4, 32, 53, 100, DateTimeKind.Local).AddTicks(6475) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 16, 4, 32, 53, 100, DateTimeKind.Local).AddTicks(6520), "Single Door", 7812m, 8, new DateTime(2025, 10, 16, 4, 32, 53, 100, DateTimeKind.Local).AddTicks(6498) });
        }
    }
}
