using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class OrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderItemId",
                table: "FridgeAllocation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 16, 15, 57, 16, 711, DateTimeKind.Local).AddTicks(7132), "Double Door", 6770m, 6, new DateTime(2025, 10, 16, 15, 57, 16, 711, DateTimeKind.Local).AddTicks(6625) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 16, 15, 57, 16, 711, DateTimeKind.Local).AddTicks(7211), "Double Door", 11533m, 3, new DateTime(2025, 10, 16, 15, 57, 16, 711, DateTimeKind.Local).AddTicks(7159) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 16, 15, 57, 16, 711, DateTimeKind.Local).AddTicks(7257), "Mini Fridge", 4649m, 9, new DateTime(2025, 10, 16, 15, 57, 16, 711, DateTimeKind.Local).AddTicks(7213) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 16, 15, 57, 16, 711, DateTimeKind.Local).AddTicks(7286), "Single Door", 8624m, 5, new DateTime(2025, 10, 16, 15, 57, 16, 711, DateTimeKind.Local).AddTicks(7260) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 16, 15, 57, 16, 711, DateTimeKind.Local).AddTicks(7312), 9074m, 9, new DateTime(2025, 10, 16, 15, 57, 16, 711, DateTimeKind.Local).AddTicks(7288) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 16, 15, 57, 16, 711, DateTimeKind.Local).AddTicks(7348), "Double Door", 7361m, 2, new DateTime(2025, 10, 16, 15, 57, 16, 711, DateTimeKind.Local).AddTicks(7322) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 16, 15, 57, 16, 711, DateTimeKind.Local).AddTicks(7374), 6266m, 5, new DateTime(2025, 10, 16, 15, 57, 16, 711, DateTimeKind.Local).AddTicks(7350) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 16, 15, 57, 16, 711, DateTimeKind.Local).AddTicks(7399), "Single Door", 5689m, 1, new DateTime(2025, 10, 16, 15, 57, 16, 711, DateTimeKind.Local).AddTicks(7376) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 16, 15, 57, 16, 711, DateTimeKind.Local).AddTicks(7471), "Double Door", 11770m, new DateTime(2025, 10, 16, 15, 57, 16, 711, DateTimeKind.Local).AddTicks(7444) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DeliveryDate", "Price", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 16, 15, 57, 16, 711, DateTimeKind.Local).AddTicks(7514), 10512m, new DateTime(2025, 10, 16, 15, 57, 16, 711, DateTimeKind.Local).AddTicks(7475) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 16, 15, 57, 16, 711, DateTimeKind.Local).AddTicks(7560), 11085m, 3, new DateTime(2025, 10, 16, 15, 57, 16, 711, DateTimeKind.Local).AddTicks(7533) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 16, 15, 57, 16, 711, DateTimeKind.Local).AddTicks(7585), "Single Door", 10231m, 9, new DateTime(2025, 10, 16, 15, 57, 16, 711, DateTimeKind.Local).AddTicks(7561) });

            migrationBuilder.CreateIndex(
                name: "IX_FridgeAllocation_OrderItemId",
                table: "FridgeAllocation",
                column: "OrderItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_FridgeAllocation_OrderItems_OrderItemId",
                table: "FridgeAllocation",
                column: "OrderItemId",
                principalTable: "OrderItems",
                principalColumn: "OrderItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FridgeAllocation_OrderItems_OrderItemId",
                table: "FridgeAllocation");

            migrationBuilder.DropIndex(
                name: "IX_FridgeAllocation_OrderItemId",
                table: "FridgeAllocation");

            migrationBuilder.DropColumn(
                name: "OrderItemId",
                table: "FridgeAllocation");

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1295), "Mini Fridge", 6329m, 4, new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1107) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1319), "Mini Fridge", 10786m, 9, new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1300) });

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
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1369), "Double Door", 11127m, 2, new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1354) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1384), 5009m, 7, new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1370) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1403), "Mini Fridge", 9494m, 3, new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1389) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1417), 5764m, 7, new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1404) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1432), "Double Door", 9995m, 8, new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1418) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1447), "Mini Fridge", 11550m, new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1433) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DeliveryDate", "Price", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1472), 8689m, new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1450) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1511), 10372m, 4, new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1496) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1544), "Double Door", 7888m, 7, new DateTime(2025, 10, 16, 6, 4, 0, 524, DateTimeKind.Local).AddTicks(1530) });
        }
    }
}
