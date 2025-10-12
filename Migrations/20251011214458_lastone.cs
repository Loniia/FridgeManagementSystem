using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class lastone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProofFilePath",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 11, 23, 44, 50, 507, DateTimeKind.Local).AddTicks(4137), "Single Door", 6813m, 7, new DateTime(2025, 10, 11, 23, 44, 50, 507, DateTimeKind.Local).AddTicks(3875) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 11, 23, 44, 50, 507, DateTimeKind.Local).AddTicks(4232), "Single Door", 3811m, 9, new DateTime(2025, 10, 11, 23, 44, 50, 507, DateTimeKind.Local).AddTicks(4141) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 11, 23, 44, 50, 507, DateTimeKind.Local).AddTicks(4275), "Single Door", 3624m, 6, new DateTime(2025, 10, 11, 23, 44, 50, 507, DateTimeKind.Local).AddTicks(4234) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 11, 23, 44, 50, 507, DateTimeKind.Local).AddTicks(4298), "Single Door", 8635m, new DateTime(2025, 10, 11, 23, 44, 50, 507, DateTimeKind.Local).AddTicks(4276) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 11, 23, 44, 50, 507, DateTimeKind.Local).AddTicks(4321), "Double Door", 10318m, 1, new DateTime(2025, 10, 11, 23, 44, 50, 507, DateTimeKind.Local).AddTicks(4299) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 11, 23, 44, 50, 507, DateTimeKind.Local).AddTicks(4350), "Double Door", 10136m, 7, new DateTime(2025, 10, 11, 23, 44, 50, 507, DateTimeKind.Local).AddTicks(4329) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 11, 23, 44, 50, 507, DateTimeKind.Local).AddTicks(4374), "Mini Fridge", 3524m, 7, new DateTime(2025, 10, 11, 23, 44, 50, 507, DateTimeKind.Local).AddTicks(4351) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 11, 23, 44, 50, 507, DateTimeKind.Local).AddTicks(4396), "Single Door", 8604m, 4, new DateTime(2025, 10, 11, 23, 44, 50, 507, DateTimeKind.Local).AddTicks(4375) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 11, 23, 44, 50, 507, DateTimeKind.Local).AddTicks(4417), "Mini Fridge", 9263m, 7, new DateTime(2025, 10, 11, 23, 44, 50, 507, DateTimeKind.Local).AddTicks(4397) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 11, 23, 44, 50, 507, DateTimeKind.Local).AddTicks(4458), "Double Door", 8669m, 2, new DateTime(2025, 10, 11, 23, 44, 50, 507, DateTimeKind.Local).AddTicks(4419) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 11, 23, 44, 50, 507, DateTimeKind.Local).AddTicks(4501), "Single Door", 10453m, 6, new DateTime(2025, 10, 11, 23, 44, 50, 507, DateTimeKind.Local).AddTicks(4478) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 11, 23, 44, 50, 507, DateTimeKind.Local).AddTicks(4532), "Single Door", 3525m, 7, new DateTime(2025, 10, 11, 23, 44, 50, 507, DateTimeKind.Local).AddTicks(4502) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProofFilePath",
                table: "Payments");

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(6920), "Double Door", 3885m, 8, new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(6640) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(6994), "Double Door", 7728m, 1, new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(6945) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(7032), "Double Door", 5896m, 1, new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(6995) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(7056), "Mini Fridge", 5732m, new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(7034) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(7077), "Single Door", 7680m, 9, new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(7059) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(7111), "Mini Fridge", 10350m, 2, new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(7091) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(7130), "Single Door", 11551m, 3, new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(7112) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(7149), "Mini Fridge", 8855m, 8, new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(7132) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(7169), "Single Door", 10499m, 5, new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(7151) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 11, 23, 10, 57, 945, DateTimeKind.Local).AddTicks(4917), "Single Door", 6657m, 8, new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(7172) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 11, 23, 10, 57, 945, DateTimeKind.Local).AddTicks(5040), "Mini Fridge", 7764m, 9, new DateTime(2025, 10, 11, 23, 10, 57, 945, DateTimeKind.Local).AddTicks(4980) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 11, 23, 10, 57, 945, DateTimeKind.Local).AddTicks(5063), "Mini Fridge", 7399m, 4, new DateTime(2025, 10, 11, 23, 10, 57, 945, DateTimeKind.Local).AddTicks(5042) });
        }
    }
}
