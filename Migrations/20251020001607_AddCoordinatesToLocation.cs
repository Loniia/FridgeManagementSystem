using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddCoordinatesToLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Locations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Locations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(7944), "Single Door", 6485m, 7, new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(7706) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8010), 6153m, 9, new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(7948) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8044), "Mini Fridge", 5855m, 4, new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8011) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8068), 5470m, 4, new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8045) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8091), "Single Door", 3707m, 9, new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8070) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8119), 8134m, 9, new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8098) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8141), 5558m, 5, new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8120) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8164), "Single Door", 11041m, 9, new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8143) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8185), 7612m, 7, new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8165) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8220), "Mini Fridge", 8227m, 9, new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8188) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8270), "Mini Fridge", 8410m, 4, new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8248) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8292), "Double Door", 5954m, 9, new DateTime(2025, 10, 20, 2, 15, 57, 618, DateTimeKind.Local).AddTicks(8272) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Locations");

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(785), "Mini Fridge", 10892m, 4, new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(337) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(863), 10807m, 3, new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(804) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(934), "Double Door", 9472m, 1, new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(865) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(963), 8607m, 8, new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(936) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(987), "Mini Fridge", 6230m, 4, new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(964) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(1016), 3793m, 1, new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(993) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(1040), 8450m, 2, new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(1018) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(1063), "Mini Fridge", 3936m, 2, new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(1041) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(1086), 7834m, 9, new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(1065) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(1131), "Double Door", 8288m, 6, new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(1089) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(1176), "Double Door", 6587m, 7, new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(1151) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(1200), "Mini Fridge", 10144m, 4, new DateTime(2025, 10, 20, 1, 42, 54, 997, DateTimeKind.Local).AddTicks(1177) });
        }
    }
}
