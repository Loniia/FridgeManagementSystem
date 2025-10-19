using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class CreateSupplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FridgeId",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderID",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "QuotationID",
                table: "Suppliers");

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 0, 26, 28, 615, DateTimeKind.Local).AddTicks(3981), "Mini Fridge", 6495m, 7, new DateTime(2025, 10, 19, 0, 26, 28, 615, DateTimeKind.Local).AddTicks(3720) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "DateAdded", "DeliveryDate", "Price", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 0, 26, 28, 615, DateTimeKind.Local).AddTicks(4022), 3999m, new DateTime(2025, 10, 19, 0, 26, 28, 615, DateTimeKind.Local).AddTicks(3990) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 0, 26, 28, 615, DateTimeKind.Local).AddTicks(4049), "Mini Fridge", 10804m, new DateTime(2025, 10, 19, 0, 26, 28, 615, DateTimeKind.Local).AddTicks(4024) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 0, 26, 28, 615, DateTimeKind.Local).AddTicks(4075), "Mini Fridge", 11461m, 4, new DateTime(2025, 10, 19, 0, 26, 28, 615, DateTimeKind.Local).AddTicks(4051) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 0, 26, 28, 615, DateTimeKind.Local).AddTicks(4130), 9598m, 2, new DateTime(2025, 10, 19, 0, 26, 28, 615, DateTimeKind.Local).AddTicks(4076) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 0, 26, 28, 615, DateTimeKind.Local).AddTicks(4170), "Mini Fridge", 6032m, 6, new DateTime(2025, 10, 19, 0, 26, 28, 615, DateTimeKind.Local).AddTicks(4136) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 0, 26, 28, 615, DateTimeKind.Local).AddTicks(4210), "Double Door", 6271m, 8, new DateTime(2025, 10, 19, 0, 26, 28, 615, DateTimeKind.Local).AddTicks(4172) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 0, 26, 28, 615, DateTimeKind.Local).AddTicks(4237), "Double Door", 9949m, new DateTime(2025, 10, 19, 0, 26, 28, 615, DateTimeKind.Local).AddTicks(4212) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 0, 26, 28, 615, DateTimeKind.Local).AddTicks(4325), "Single Door", 8388m, 3, new DateTime(2025, 10, 19, 0, 26, 28, 615, DateTimeKind.Local).AddTicks(4238) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 0, 26, 28, 615, DateTimeKind.Local).AddTicks(4376), "Mini Fridge", 4003m, 9, new DateTime(2025, 10, 19, 0, 26, 28, 615, DateTimeKind.Local).AddTicks(4330) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 0, 26, 28, 615, DateTimeKind.Local).AddTicks(4424), "Double Door", 9860m, 7, new DateTime(2025, 10, 19, 0, 26, 28, 615, DateTimeKind.Local).AddTicks(4395) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 0, 26, 28, 615, DateTimeKind.Local).AddTicks(4450), "Mini Fridge", 4556m, 2, new DateTime(2025, 10, 19, 0, 26, 28, 615, DateTimeKind.Local).AddTicks(4425) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FridgeId",
                table: "Suppliers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrderID",
                table: "Suppliers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuotationID",
                table: "Suppliers",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                columns: new[] { "DateAdded", "DeliveryDate", "Price", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1444), 8579m, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1394) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1498), "Double Door", 7687m, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1448) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1933), "Single Door", 7378m, 1, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1881) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1965), 5826m, 3, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1936) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2007), "Double Door", 10428m, 8, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1976) });

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
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2072), "Mini Fridge", 3848m, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2043) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2102), "Double Door", 8928m, 7, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2074) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2154), "Single Door", 6910m, 1, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2106) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2212), "Mini Fridge", 7766m, 4, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2181) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2240), "Double Door", 8323m, 3, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2214) });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "SupplierID",
                keyValue: 1,
                columns: new[] { "FridgeId", "PurchaseOrderID", "QuotationID" },
                values: new object[] { 0, 0, 0 });
        }
    }
}
