using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class FridgeDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6569), "Double Door", 11726m, new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6398) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6589), "Mini Fridge", 11047m, new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6572) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6605), "Mini Fridge", 7960m, 2, new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6590) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6620), "Double Door", 10754m, new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6606) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6635), "Single Door", 10298m, 4, new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6621) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6652), "Single Door", 4284m, 6, new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6639) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6666), 11588m, 2, new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6654) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6680), 9909m, 4, new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6667) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6693), 8258m, 6, new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6681) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6712), "Mini Fridge", 11889m, 4, new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6696) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6726), "Mini Fridge", 9390m, 3, new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6713) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6758), "Double Door", 11048m, 4, new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6728) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 10, 8, 46, 14, 370, DateTimeKind.Local).AddTicks(9594), "Mini Fridge", 8248m, new DateTime(2025, 10, 10, 8, 46, 14, 370, DateTimeKind.Local).AddTicks(9357) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 10, 8, 46, 14, 370, DateTimeKind.Local).AddTicks(9647), "Single Door", 3631m, new DateTime(2025, 10, 10, 8, 46, 14, 370, DateTimeKind.Local).AddTicks(9604) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 10, 8, 46, 14, 370, DateTimeKind.Local).AddTicks(9686), "Single Door", 11861m, 3, new DateTime(2025, 10, 10, 8, 46, 14, 370, DateTimeKind.Local).AddTicks(9650) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 10, 8, 46, 14, 370, DateTimeKind.Local).AddTicks(9714), "Mini Fridge", 5877m, new DateTime(2025, 10, 10, 8, 46, 14, 370, DateTimeKind.Local).AddTicks(9689) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 10, 8, 46, 14, 370, DateTimeKind.Local).AddTicks(9737), "Mini Fridge", 8604m, 1, new DateTime(2025, 10, 10, 8, 46, 14, 370, DateTimeKind.Local).AddTicks(9716) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 10, 8, 46, 14, 370, DateTimeKind.Local).AddTicks(9768), "Mini Fridge", 10994m, 5, new DateTime(2025, 10, 10, 8, 46, 14, 370, DateTimeKind.Local).AddTicks(9747) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 10, 8, 46, 14, 370, DateTimeKind.Local).AddTicks(9791), 7387m, 5, new DateTime(2025, 10, 10, 8, 46, 14, 370, DateTimeKind.Local).AddTicks(9770) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 10, 8, 46, 14, 370, DateTimeKind.Local).AddTicks(9814), 11967m, 6, new DateTime(2025, 10, 10, 8, 46, 14, 370, DateTimeKind.Local).AddTicks(9793) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 10, 8, 46, 14, 370, DateTimeKind.Local).AddTicks(9838), 8029m, 1, new DateTime(2025, 10, 10, 8, 46, 14, 370, DateTimeKind.Local).AddTicks(9817) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 10, 8, 46, 14, 370, DateTimeKind.Local).AddTicks(9881), "Single Door", 7959m, 6, new DateTime(2025, 10, 10, 8, 46, 14, 370, DateTimeKind.Local).AddTicks(9846) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 10, 8, 46, 14, 370, DateTimeKind.Local).AddTicks(9930), "Single Door", 10325m, 9, new DateTime(2025, 10, 10, 8, 46, 14, 370, DateTimeKind.Local).AddTicks(9909) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 10, 8, 46, 14, 370, DateTimeKind.Local).AddTicks(9953), "Mini Fridge", 7180m, 7, new DateTime(2025, 10, 10, 8, 46, 14, 370, DateTimeKind.Local).AddTicks(9932) });
        }
    }
}
