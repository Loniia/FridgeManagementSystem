using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseRequestColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Quotations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RequiredQuantity",
                table: "Quotations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovalDate",
                table: "PurchaseRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovalNotes",
                table: "PurchaseRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedRFQID",
                table: "PurchaseRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "PurchaseRequests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 19, 59, 58, 925, DateTimeKind.Local).AddTicks(6900), "Double Door", 9732m, 3, new DateTime(2025, 10, 19, 19, 59, 58, 925, DateTimeKind.Local).AddTicks(6721) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 19, 59, 58, 925, DateTimeKind.Local).AddTicks(6931), "Mini Fridge", 10346m, 8, new DateTime(2025, 10, 19, 19, 59, 58, 925, DateTimeKind.Local).AddTicks(6921) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 19, 59, 58, 925, DateTimeKind.Local).AddTicks(6940), "Mini Fridge", 5174m, 3, new DateTime(2025, 10, 19, 19, 59, 58, 925, DateTimeKind.Local).AddTicks(6932) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 19, 59, 58, 925, DateTimeKind.Local).AddTicks(6949), 7524m, 9, new DateTime(2025, 10, 19, 19, 59, 58, 925, DateTimeKind.Local).AddTicks(6941) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 19, 59, 58, 925, DateTimeKind.Local).AddTicks(6957), "Mini Fridge", 4734m, new DateTime(2025, 10, 19, 19, 59, 58, 925, DateTimeKind.Local).AddTicks(6950) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 19, 59, 58, 925, DateTimeKind.Local).AddTicks(6970), "Mini Fridge", 9264m, 3, new DateTime(2025, 10, 19, 19, 59, 58, 925, DateTimeKind.Local).AddTicks(6962) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 19, 59, 58, 925, DateTimeKind.Local).AddTicks(6979), 10765m, 5, new DateTime(2025, 10, 19, 19, 59, 58, 925, DateTimeKind.Local).AddTicks(6971) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 19, 59, 58, 925, DateTimeKind.Local).AddTicks(6987), "Single Door", 9719m, 4, new DateTime(2025, 10, 19, 19, 59, 58, 925, DateTimeKind.Local).AddTicks(6980) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 19, 59, 58, 925, DateTimeKind.Local).AddTicks(6996), 11230m, 9, new DateTime(2025, 10, 19, 19, 59, 58, 925, DateTimeKind.Local).AddTicks(6988) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 19, 59, 58, 925, DateTimeKind.Local).AddTicks(7044), "Double Door", 11723m, 9, new DateTime(2025, 10, 19, 19, 59, 58, 925, DateTimeKind.Local).AddTicks(7012) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 19, 59, 58, 925, DateTimeKind.Local).AddTicks(7080), "Double Door", 11340m, 5, new DateTime(2025, 10, 19, 19, 59, 58, 925, DateTimeKind.Local).AddTicks(7060) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 19, 59, 58, 925, DateTimeKind.Local).AddTicks(7107), "Single Door", 11113m, 4, new DateTime(2025, 10, 19, 19, 59, 58, 925, DateTimeKind.Local).AddTicks(7082) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Quotations");

            migrationBuilder.DropColumn(
                name: "RequiredQuantity",
                table: "Quotations");

            migrationBuilder.DropColumn(
                name: "ApprovalDate",
                table: "PurchaseRequests");

            migrationBuilder.DropColumn(
                name: "ApprovalNotes",
                table: "PurchaseRequests");

            migrationBuilder.DropColumn(
                name: "CreatedRFQID",
                table: "PurchaseRequests");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "PurchaseRequests");

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
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1444), "Single Door", 8579m, 4, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1394) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1498), "Double Door", 7687m, 9, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1448) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1933), 7378m, 1, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1881) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1965), "Single Door", 5826m, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1936) });

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
                columns: new[] { "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2041), 7550m, 7, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2009) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2072), "Mini Fridge", 3848m, 5, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2043) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2102), 8928m, 7, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2074) });

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
