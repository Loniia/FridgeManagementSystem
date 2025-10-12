using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePaymentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ProofOfPaymentPath",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "ReferenceCode",
                table: "Payments",
                newName: "PaymentReference");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

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
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(7032), "Double Door", 5896m, 1, new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(6995) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DeliveryDate", "Price", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(7056), 5732m, new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(7034) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(7077), 7680m, 9, new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(7059) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(7111), "Mini Fridge", 10350m, 2, new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(7091) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(7130), 11551m, 3, new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(7112) });

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
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(7169), "Single Door", 10499m, 5, new DateTime(2025, 10, 11, 23, 10, 57, 944, DateTimeKind.Local).AddTicks(7151) });

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
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 11, 23, 10, 57, 945, DateTimeKind.Local).AddTicks(5040), 7764m, 9, new DateTime(2025, 10, 11, 23, 10, 57, 945, DateTimeKind.Local).AddTicks(4980) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 11, 23, 10, 57, 945, DateTimeKind.Local).AddTicks(5063), 7399m, 4, new DateTime(2025, 10, 11, 23, 10, 57, 945, DateTimeKind.Local).AddTicks(5042) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentReference",
                table: "Payments",
                newName: "ReferenceCode");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Payments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ProofOfPaymentPath",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8224), "Single Door", 5945m, 5, new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(7735) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8253), "Mini Fridge", 9876m, 8, new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8231) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8296), "Mini Fridge", 9049m, 5, new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8255) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DeliveryDate", "Price", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8317), 7454m, new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8298) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8336), 6157m, 5, new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8318) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8363), "Single Door", 5566m, 9, new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8344) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8408), 8566m, 1, new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8365) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8430), "Double Door", 7994m, 1, new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8410) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8451), "Mini Fridge", 6211m, 7, new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8432) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8494), "Mini Fridge", 4722m, 5, new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8456) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8542), 9185m, 5, new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8521) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8561), 4846m, 6, new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8543) });
        }
    }
}
