using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class notification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 41, 22, 294, DateTimeKind.Local).AddTicks(7357), "Single Door", 5889m, 5, new DateTime(2025, 10, 25, 0, 41, 22, 294, DateTimeKind.Local).AddTicks(7015) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 41, 22, 294, DateTimeKind.Local).AddTicks(7408), "Double Door", 9399m, 7, new DateTime(2025, 10, 25, 0, 41, 22, 294, DateTimeKind.Local).AddTicks(7380) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 41, 22, 294, DateTimeKind.Local).AddTicks(7470), "Single Door", 11507m, 1, new DateTime(2025, 10, 25, 0, 41, 22, 294, DateTimeKind.Local).AddTicks(7411) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 41, 22, 294, DateTimeKind.Local).AddTicks(7495), "Double Door", 4914m, 9, new DateTime(2025, 10, 25, 0, 41, 22, 294, DateTimeKind.Local).AddTicks(7472) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 41, 22, 294, DateTimeKind.Local).AddTicks(7516), 9072m, 1, new DateTime(2025, 10, 25, 0, 41, 22, 294, DateTimeKind.Local).AddTicks(7496) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 41, 22, 294, DateTimeKind.Local).AddTicks(8093), "Mini Fridge", 5564m, 8, new DateTime(2025, 10, 25, 0, 41, 22, 294, DateTimeKind.Local).AddTicks(8031) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 41, 22, 294, DateTimeKind.Local).AddTicks(8119), "Single Door", 11256m, 5, new DateTime(2025, 10, 25, 0, 41, 22, 294, DateTimeKind.Local).AddTicks(8098) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 41, 22, 294, DateTimeKind.Local).AddTicks(8140), "Mini Fridge", 10973m, 8, new DateTime(2025, 10, 25, 0, 41, 22, 294, DateTimeKind.Local).AddTicks(8120) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 41, 22, 294, DateTimeKind.Local).AddTicks(8163), "Double Door", 4118m, 4, new DateTime(2025, 10, 25, 0, 41, 22, 294, DateTimeKind.Local).AddTicks(8142) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 41, 22, 294, DateTimeKind.Local).AddTicks(8204), "Double Door", 7358m, 8, new DateTime(2025, 10, 25, 0, 41, 22, 294, DateTimeKind.Local).AddTicks(8169) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 41, 22, 294, DateTimeKind.Local).AddTicks(8250), "Mini Fridge", 8055m, 6, new DateTime(2025, 10, 25, 0, 41, 22, 294, DateTimeKind.Local).AddTicks(8229) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 41, 22, 294, DateTimeKind.Local).AddTicks(8272), 7888m, 4, new DateTime(2025, 10, 25, 0, 41, 22, 294, DateTimeKind.Local).AddTicks(8252) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 24), new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(1847), "Mini Fridge", 11834m, 9, new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(1493) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 24), new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(1910), "Single Door", 6335m, 4, new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(1853) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 24), new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(1944), "Double Door", 6400m, 9, new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(1912) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 24), new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(1968), "Single Door", 6810m, 7, new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(1946) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 24), new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(1991), 10476m, 2, new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(1970) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 24), new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(2019), "Single Door", 6450m, 1, new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(1999) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 24), new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(2041), "Double Door", 5093m, 6, new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(2021) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 24), new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(2062), "Single Door", 6581m, 5, new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(2042) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 24), new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(2083), "Mini Fridge", 11707m, 2, new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(2063) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 24), new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(2118), "Single Door", 3644m, 5, new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(2087) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 24), new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(2248), "Double Door", 9576m, 7, new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(2224) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 24), new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(2270), 7122m, 9, new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(2250) });
        }
    }
}
