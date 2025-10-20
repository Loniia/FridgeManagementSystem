using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class CompletedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedDate",
                table: "MaintenanceRequest",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 20), new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(785), "Mini Fridge", 10892m, 4, new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(337) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 20), new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(863), "Double Door", 10807m, 3, new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(804) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 20), new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(934), "Double Door", 9472m, new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(865) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 20), new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(963), "Mini Fridge", 8607m, 8, new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(936) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 20), new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(987), 6230m, 4, new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(964) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 20), new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(1016), "Single Door", 3793m, 1, new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(993) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 20), new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(1040), 8450m, 2, new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(1018) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 20), new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(1063), "Mini Fridge", 3936m, 2, new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(1041) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 20), new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(1086), 7834m, 9, new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(1065) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 20), new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(1131), 8288m, 6, new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(1089) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 20), new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(1176), "Double Door", 6587m, 7, new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(1151) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 20), new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(1200), "Mini Fridge", 10144m, 4, new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(1177) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedDate",
                table: "MaintenanceRequest");

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(6863), "Double Door", 4328m, 6, new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(6682) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(6886), "Mini Fridge", 5985m, 5, new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(6867) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(6935), "Mini Fridge", 5086m, new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(6887) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(6953), "Single Door", 9150m, 4, new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(6936) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(6971), 10688m, 6, new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(6954) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(6993), "Mini Fridge", 8217m, 4, new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(6977) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(7011), 8163m, 1, new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(6995) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(7028), "Double Door", 10099m, 8, new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(7012) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(7045), 11905m, 4, new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(7029) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(7079), 10896m, 7, new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(7048) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(7113), "Single Door", 4416m, 8, new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(7096) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(7130), "Single Door", 11922m, 8, new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(7114) });
        }
    }
}
