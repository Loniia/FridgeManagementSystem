using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class PaymentNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5379), 9673m, 8, new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5200) });

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
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5434), 8916m, 1, new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5404) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5451), "Mini Fridge", 8996m, 8, new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5435) });

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
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5542), "Mini Fridge", 5585m, new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5526) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5556), 6475m, 9, new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5543) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5587), 4984m, 1, new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5560) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5684), "Single Door", 6011m, 7, new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5667) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5699), 4569m, 8, new DateTime(2025, 10, 12, 18, 3, 29, 886, DateTimeKind.Local).AddTicks(5685) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 12, 17, 11, 51, 694, DateTimeKind.Local).AddTicks(8324), 7381m, 1, new DateTime(2025, 10, 12, 17, 11, 51, 694, DateTimeKind.Local).AddTicks(7951) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 12, 17, 11, 51, 694, DateTimeKind.Local).AddTicks(8410), 11410m, 6, new DateTime(2025, 10, 12, 17, 11, 51, 694, DateTimeKind.Local).AddTicks(8360) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 12, 17, 11, 51, 694, DateTimeKind.Local).AddTicks(8454), 10388m, 4, new DateTime(2025, 10, 12, 17, 11, 51, 694, DateTimeKind.Local).AddTicks(8413) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 12, 17, 11, 51, 694, DateTimeKind.Local).AddTicks(8482), "Double Door", 9664m, 6, new DateTime(2025, 10, 12, 17, 11, 51, 694, DateTimeKind.Local).AddTicks(8457) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 12, 17, 11, 51, 694, DateTimeKind.Local).AddTicks(8508), "Single Door", 10886m, 6, new DateTime(2025, 10, 12, 17, 11, 51, 694, DateTimeKind.Local).AddTicks(8484) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 12, 17, 11, 51, 694, DateTimeKind.Local).AddTicks(8541), "Double Door", 4578m, 3, new DateTime(2025, 10, 12, 17, 11, 51, 694, DateTimeKind.Local).AddTicks(8516) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 12, 17, 11, 51, 694, DateTimeKind.Local).AddTicks(8565), "Double Door", 6382m, 6, new DateTime(2025, 10, 12, 17, 11, 51, 694, DateTimeKind.Local).AddTicks(8542) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 12, 17, 11, 51, 694, DateTimeKind.Local).AddTicks(8589), "Double Door", 5706m, new DateTime(2025, 10, 12, 17, 11, 51, 694, DateTimeKind.Local).AddTicks(8567) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 12, 17, 11, 51, 694, DateTimeKind.Local).AddTicks(8614), 11982m, 4, new DateTime(2025, 10, 12, 17, 11, 51, 694, DateTimeKind.Local).AddTicks(8590) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 12, 17, 11, 51, 694, DateTimeKind.Local).AddTicks(8672), 7726m, 8, new DateTime(2025, 10, 12, 17, 11, 51, 694, DateTimeKind.Local).AddTicks(8618) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 12, 17, 11, 51, 694, DateTimeKind.Local).AddTicks(8717), "Mini Fridge", 6478m, 4, new DateTime(2025, 10, 12, 17, 11, 51, 694, DateTimeKind.Local).AddTicks(8692) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 12, 17, 11, 51, 694, DateTimeKind.Local).AddTicks(8742), 8452m, 9, new DateTime(2025, 10, 12, 17, 11, 51, 694, DateTimeKind.Local).AddTicks(8719) });
        }
    }
}
