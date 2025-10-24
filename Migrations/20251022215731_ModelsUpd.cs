using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class ModelsUpd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 22, 23, 57, 27, 193, DateTimeKind.Local).AddTicks(8801), "Mini Fridge", 11768m, 8, new DateTime(2025, 10, 22, 23, 57, 27, 193, DateTimeKind.Local).AddTicks(8654) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 22, 23, 57, 27, 193, DateTimeKind.Local).AddTicks(8814), "Single Door", 11811m, 6, new DateTime(2025, 10, 22, 23, 57, 27, 193, DateTimeKind.Local).AddTicks(8806) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 22, 23, 57, 27, 193, DateTimeKind.Local).AddTicks(8820), "Mini Fridge", 9605m, new DateTime(2025, 10, 22, 23, 57, 27, 193, DateTimeKind.Local).AddTicks(8815) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DeliveryDate", "Price", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 22, 23, 57, 27, 193, DateTimeKind.Local).AddTicks(8826), 4107m, new DateTime(2025, 10, 22, 23, 57, 27, 193, DateTimeKind.Local).AddTicks(8821) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 22, 23, 57, 27, 193, DateTimeKind.Local).AddTicks(8831), "Mini Fridge", 11180m, 4, new DateTime(2025, 10, 22, 23, 57, 27, 193, DateTimeKind.Local).AddTicks(8826) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 22, 23, 57, 27, 193, DateTimeKind.Local).AddTicks(8840), "Double Door", 8642m, 4, new DateTime(2025, 10, 22, 23, 57, 27, 193, DateTimeKind.Local).AddTicks(8834) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 22, 23, 57, 27, 193, DateTimeKind.Local).AddTicks(8845), "Mini Fridge", 5129m, 6, new DateTime(2025, 10, 22, 23, 57, 27, 193, DateTimeKind.Local).AddTicks(8841) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 22, 23, 57, 27, 193, DateTimeKind.Local).AddTicks(8851), "Mini Fridge", 5840m, 3, new DateTime(2025, 10, 22, 23, 57, 27, 193, DateTimeKind.Local).AddTicks(8846) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 22, 23, 57, 27, 193, DateTimeKind.Local).AddTicks(8856), 9286m, 7, new DateTime(2025, 10, 22, 23, 57, 27, 193, DateTimeKind.Local).AddTicks(8851) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 22, 23, 57, 27, 193, DateTimeKind.Local).AddTicks(8888), 4148m, 8, new DateTime(2025, 10, 22, 23, 57, 27, 193, DateTimeKind.Local).AddTicks(8871) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 22, 23, 57, 27, 193, DateTimeKind.Local).AddTicks(8905), "Mini Fridge", 7448m, 4, new DateTime(2025, 10, 22, 23, 57, 27, 193, DateTimeKind.Local).AddTicks(8900) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DeliveryDate", "Price", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 22, 23, 57, 27, 193, DateTimeKind.Local).AddTicks(8911), 11683m, new DateTime(2025, 10, 22, 23, 57, 27, 193, DateTimeKind.Local).AddTicks(8906) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2014), "Single Door", 5211m, 2, new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(1798) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2037), "Double Door", 7374m, 4, new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2020) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2045), "Single Door", 10706m, new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2038) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DeliveryDate", "Price", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2052), 7100m, new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2046) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2059), "Double Door", 8423m, 1, new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2053) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2071), "Mini Fridge", 11819m, 5, new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2065) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2079), "Single Door", 10967m, 1, new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2073) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2086), "Double Door", 10908m, 9, new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2080) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2093), 3833m, 9, new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2087) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2136), 4191m, 6, new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2114) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2165), "Single Door", 7741m, 7, new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2157) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DeliveryDate", "Price", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2178), 10441m, new DateTime(2025, 10, 22, 20, 33, 29, 520, DateTimeKind.Local).AddTicks(2166) });
        }
    }
}
