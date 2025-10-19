using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class OPF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FaultID",
                table: "FaultReport",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 19, 17, 57, 20, 952, DateTimeKind.Local).AddTicks(3743), "Mini Fridge", 8143m, 7, new DateTime(2025, 10, 19, 17, 57, 20, 952, DateTimeKind.Local).AddTicks(3033) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 19, 17, 57, 20, 952, DateTimeKind.Local).AddTicks(3780), 4155m, 1, new DateTime(2025, 10, 19, 17, 57, 20, 952, DateTimeKind.Local).AddTicks(3754) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 19, 17, 57, 20, 952, DateTimeKind.Local).AddTicks(3818), "Double Door", 9059m, 7, new DateTime(2025, 10, 19, 17, 57, 20, 952, DateTimeKind.Local).AddTicks(3782) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 19, 17, 57, 20, 952, DateTimeKind.Local).AddTicks(3837), 3991m, 6, new DateTime(2025, 10, 19, 17, 57, 20, 952, DateTimeKind.Local).AddTicks(3819) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 19, 17, 57, 20, 952, DateTimeKind.Local).AddTicks(3856), 11782m, 2, new DateTime(2025, 10, 19, 17, 57, 20, 952, DateTimeKind.Local).AddTicks(3839) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 19, 17, 57, 20, 952, DateTimeKind.Local).AddTicks(3990), "Single Door", 8111m, 8, new DateTime(2025, 10, 19, 17, 57, 20, 952, DateTimeKind.Local).AddTicks(3862) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 19, 17, 57, 20, 952, DateTimeKind.Local).AddTicks(4010), "Double Door", 6569m, 3, new DateTime(2025, 10, 19, 17, 57, 20, 952, DateTimeKind.Local).AddTicks(3992) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 19, 17, 57, 20, 952, DateTimeKind.Local).AddTicks(4027), "Single Door", 4614m, 6, new DateTime(2025, 10, 19, 17, 57, 20, 952, DateTimeKind.Local).AddTicks(4011) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 19, 17, 57, 20, 952, DateTimeKind.Local).AddTicks(4044), "Double Door", 5572m, 3, new DateTime(2025, 10, 19, 17, 57, 20, 952, DateTimeKind.Local).AddTicks(4028) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 19, 17, 57, 20, 952, DateTimeKind.Local).AddTicks(4078), 8963m, 1, new DateTime(2025, 10, 19, 17, 57, 20, 952, DateTimeKind.Local).AddTicks(4049) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 19, 17, 57, 20, 952, DateTimeKind.Local).AddTicks(4110), "Double Door", 3890m, 2, new DateTime(2025, 10, 19, 17, 57, 20, 952, DateTimeKind.Local).AddTicks(4093) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 19, 17, 57, 20, 952, DateTimeKind.Local).AddTicks(4128), 7672m, 8, new DateTime(2025, 10, 19, 17, 57, 20, 952, DateTimeKind.Local).AddTicks(4112) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FaultID",
                table: "FaultReport",
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
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1183), "Double Door", 7681m, 1, new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(988) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1209), 5524m, 2, new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1187) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1229), "Mini Fridge", 10927m, 4, new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1210) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1248), 4512m, 3, new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1230) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1267), 5055m, 1, new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1249) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1292), "Mini Fridge", 8356m, 1, new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1273) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1353), "Mini Fridge", 11451m, 2, new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1310) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1376), "Mini Fridge", 5381m, 1, new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1355) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1401), "Single Door", 8848m, 4, new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1377) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1446), 6134m, 9, new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1405) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1482), "Single Door", 3758m, 1, new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1461) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1502), 8232m, 9, new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1483) });
        }
    }
}
