using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class FaultReporting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1183), "Double Door", 7681m, 1, new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(988) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1209), 5524m, 2, new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1187) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1229), "Mini Fridge", 10927m, 4, new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1210) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1248), "Double Door", 4512m, 3, new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1230) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1267), 5055m, 1, new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1249) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1292), "Mini Fridge", 8356m, 1, new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1273) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1353), "Mini Fridge", 11451m, 2, new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1310) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1376), 5381m, 1, new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1355) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1401), "Single Door", 8848m, 4, new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1377) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1446), "Double Door", 6134m, 9, new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1405) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1482), "Single Door", 3758m, 1, new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1461) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1502), "Single Door", 8232m, 9, new DateTime(2025, 10, 19, 17, 31, 18, 409, DateTimeKind.Local).AddTicks(1483) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1368), "Single Door", 7692m, 9, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(918) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1444), 8579m, 4, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1394) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1498), "Double Door", 7687m, 9, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1448) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1933), "Single Door", 7378m, 1, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1881) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1965), 5826m, 3, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1936) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2007), "Double Door", 10428m, 8, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1976) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2041), "Single Door", 7550m, 7, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2009) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2072), 3848m, 5, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2043) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2102), "Double Door", 8928m, 7, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2074) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2154), "Single Door", 6910m, 1, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2106) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2212), "Mini Fridge", 7766m, 4, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2181) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2240), "Double Door", 8323m, 3, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2214) });
        }
    }
}
