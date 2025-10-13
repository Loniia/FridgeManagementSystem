using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class RemovePayPal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayPalTransactionId",
                table: "Payments");

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 12, 23, 26, 41, 920, DateTimeKind.Local).AddTicks(7579), 3725m, 5, new DateTime(2025, 10, 12, 23, 26, 41, 920, DateTimeKind.Local).AddTicks(7373) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 12, 23, 26, 41, 920, DateTimeKind.Local).AddTicks(7639), 5571m, 7, new DateTime(2025, 10, 12, 23, 26, 41, 920, DateTimeKind.Local).AddTicks(7602) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 12, 23, 26, 41, 920, DateTimeKind.Local).AddTicks(7732), "Mini Fridge", 8878m, 2, new DateTime(2025, 10, 12, 23, 26, 41, 920, DateTimeKind.Local).AddTicks(7641) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 12, 23, 26, 41, 920, DateTimeKind.Local).AddTicks(7754), "Double Door", 5194m, 5, new DateTime(2025, 10, 12, 23, 26, 41, 920, DateTimeKind.Local).AddTicks(7733) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 12, 23, 26, 41, 920, DateTimeKind.Local).AddTicks(7804), "Single Door", 8308m, 5, new DateTime(2025, 10, 12, 23, 26, 41, 920, DateTimeKind.Local).AddTicks(7783) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 12, 23, 26, 41, 920, DateTimeKind.Local).AddTicks(7829), "Double Door", 11693m, 2, new DateTime(2025, 10, 12, 23, 26, 41, 920, DateTimeKind.Local).AddTicks(7810) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 12, 23, 26, 41, 920, DateTimeKind.Local).AddTicks(7848), "Single Door", 7380m, 6, new DateTime(2025, 10, 12, 23, 26, 41, 920, DateTimeKind.Local).AddTicks(7830) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 12, 23, 26, 41, 920, DateTimeKind.Local).AddTicks(7868), "Double Door", 11592m, 9, new DateTime(2025, 10, 12, 23, 26, 41, 920, DateTimeKind.Local).AddTicks(7850) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 12, 23, 26, 41, 920, DateTimeKind.Local).AddTicks(7886), "Mini Fridge", 8385m, 2, new DateTime(2025, 10, 12, 23, 26, 41, 920, DateTimeKind.Local).AddTicks(7869) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 12, 23, 26, 41, 920, DateTimeKind.Local).AddTicks(7919), "Double Door", 8082m, 8, new DateTime(2025, 10, 12, 23, 26, 41, 920, DateTimeKind.Local).AddTicks(7889) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 12, 23, 26, 41, 920, DateTimeKind.Local).AddTicks(7978), 9656m, 4, new DateTime(2025, 10, 12, 23, 26, 41, 920, DateTimeKind.Local).AddTicks(7957) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 12, 23, 26, 41, 920, DateTimeKind.Local).AddTicks(7998), "Single Door", 11241m, new DateTime(2025, 10, 12, 23, 26, 41, 920, DateTimeKind.Local).AddTicks(7979) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PayPalTransactionId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5379), 9673m, 8, new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5200) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5403), 8771m, 1, new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5384) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5434), "Single Door", 8916m, 1, new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5404) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5451), "Mini Fridge", 8996m, 8, new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5435) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5467), "Mini Fridge", 6987m, 7, new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5452) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5488), "Mini Fridge", 9017m, 9, new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5473) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5525), "Mini Fridge", 11334m, 9, new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5509) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5542), "Mini Fridge", 5585m, 4, new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5526) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5556), "Single Door", 6475m, 9, new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5543) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5587), "Single Door", 4984m, 1, new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5560) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5684), 6011m, 7, new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5667) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5699), "Double Door", 4569m, new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5685) });
        }
    }
}
