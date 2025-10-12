using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class PayPal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FridgeId",
                table: "PurchaseRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "AllocationStatus",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 12), new DateTime(2025, 10, 12, 2, 31, 37, 219, DateTimeKind.Local).AddTicks(246), "Single Door", 5407m, 6, new DateTime(2025, 10, 12, 2, 31, 37, 218, DateTimeKind.Local).AddTicks(9839) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 12), new DateTime(2025, 10, 12, 2, 31, 37, 219, DateTimeKind.Local).AddTicks(281), "Mini Fridge", 3572m, 3, new DateTime(2025, 10, 12, 2, 31, 37, 219, DateTimeKind.Local).AddTicks(255) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 12), new DateTime(2025, 10, 12, 2, 31, 37, 219, DateTimeKind.Local).AddTicks(307), 5716m, 4, new DateTime(2025, 10, 12, 2, 31, 37, 219, DateTimeKind.Local).AddTicks(283) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 12), new DateTime(2025, 10, 12, 2, 31, 37, 219, DateTimeKind.Local).AddTicks(362), "Mini Fridge", 10302m, 7, new DateTime(2025, 10, 12, 2, 31, 37, 219, DateTimeKind.Local).AddTicks(308) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 12), new DateTime(2025, 10, 12, 2, 31, 37, 219, DateTimeKind.Local).AddTicks(408), "Single Door", 7200m, 3, new DateTime(2025, 10, 12, 2, 31, 37, 219, DateTimeKind.Local).AddTicks(364) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 12), new DateTime(2025, 10, 12, 2, 31, 37, 219, DateTimeKind.Local).AddTicks(439), "Single Door", 5067m, new DateTime(2025, 10, 12, 2, 31, 37, 219, DateTimeKind.Local).AddTicks(415) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 12), new DateTime(2025, 10, 12, 2, 31, 37, 219, DateTimeKind.Local).AddTicks(461), "Mini Fridge", 6070m, 2, new DateTime(2025, 10, 12, 2, 31, 37, 219, DateTimeKind.Local).AddTicks(440) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 12), new DateTime(2025, 10, 12, 2, 31, 37, 219, DateTimeKind.Local).AddTicks(483), 9211m, 2, new DateTime(2025, 10, 12, 2, 31, 37, 219, DateTimeKind.Local).AddTicks(463) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 12), new DateTime(2025, 10, 12, 2, 31, 37, 219, DateTimeKind.Local).AddTicks(512), "Double Door", 8895m, new DateTime(2025, 10, 12, 2, 31, 37, 219, DateTimeKind.Local).AddTicks(485) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 12), new DateTime(2025, 10, 12, 2, 31, 37, 219, DateTimeKind.Local).AddTicks(556), "Double Door", 6127m, new DateTime(2025, 10, 12, 2, 31, 37, 219, DateTimeKind.Local).AddTicks(518) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 12), new DateTime(2025, 10, 12, 2, 31, 37, 219, DateTimeKind.Local).AddTicks(613), "Single Door", 4142m, 5, new DateTime(2025, 10, 12, 2, 31, 37, 219, DateTimeKind.Local).AddTicks(591) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 12), new DateTime(2025, 10, 12, 2, 31, 37, 219, DateTimeKind.Local).AddTicks(635), "Double Door", 7604m, 7, new DateTime(2025, 10, 12, 2, 31, 37, 219, DateTimeKind.Local).AddTicks(614) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllocationStatus",
                table: "OrderItems");

            migrationBuilder.AlterColumn<int>(
                name: "FridgeId",
                table: "PurchaseRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 11), new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2111), "Mini Fridge", 10814m, 5, new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(1870) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 11), new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2142), "Single Door", 11746m, 4, new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2117) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 11), new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2164), 4169m, 8, new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2144) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 11), new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2185), "Double Door", 11274m, 8, new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2166) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 11), new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2207), "Mini Fridge", 11281m, 1, new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2187) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 11), new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2234), "Mini Fridge", 9679m, new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2213) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 11), new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2254), "Double Door", 9170m, 6, new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2236) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 11), new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2275), 5955m, 8, new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2256) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 11), new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2306), "Single Door", 6521m, new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2277) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 11), new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2350), "Single Door", 9439m, new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2311) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 11), new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2402), "Double Door", 9209m, 4, new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2380) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 11), new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2423), "Mini Fridge", 4331m, 3, new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2403) });
        }
    }
}
