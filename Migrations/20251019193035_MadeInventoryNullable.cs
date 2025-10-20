using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class MadeInventoryNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomersCustomerID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomersCustomerID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CustomersCustomerID",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "InventoryID",
                table: "PurchaseRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "InventoryID1",
                table: "PurchaseRequests",
                type: "int",
                nullable: true);

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
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(6886), "Mini Fridge", 5985m, 5, new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(6867) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(6935), 5086m, 1, new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(6887) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(6953), 9150m, 4, new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(6936) });

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
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(6993), 8217m, 4, new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(6977) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(7011), "Single Door", 8163m, 1, new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(6995) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(7028), "Double Door", 10099m, 8, new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(7012) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(7045), "Double Door", 11905m, 4, new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(7029) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 19), new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(7079), "Double Door", 10896m, 7, new DateTime(2025, 10, 19, 21, 30, 32, 569, DateTimeKind.Local).AddTicks(7048) });

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

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_InventoryID1",
                table: "PurchaseRequests",
                column: "InventoryID1");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequests_Inventory_InventoryID1",
                table: "PurchaseRequests",
                column: "InventoryID1",
                principalTable: "Inventory",
                principalColumn: "InventoryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_Inventory_InventoryID1",
                table: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequests_InventoryID1",
                table: "PurchaseRequests");

            migrationBuilder.DropColumn(
                name: "InventoryID1",
                table: "PurchaseRequests");

            migrationBuilder.AlterColumn<int>(
                name: "InventoryID",
                table: "PurchaseRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomersCustomerID",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1368), "Mini Fridge", 7692m, 7, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(918) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1444), "Single Door", 8579m, 1, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1394) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1498), 7687m, 7, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1448) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1933), 7378m, 1, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1881) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1965), 5826m, 2, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1936) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2007), 10428m, 6, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(1976) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2041), "Double Door", 7550m, 7, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2009) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2072), "Mini Fridge", 3848m, 6, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2043) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2102), "Single Door", 8928m, 3, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2074) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2154), "Single Door", 6910m, 9, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2106) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2212), "Double Door", 7766m, 4, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2181) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 17), new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2240), "Double Door", 8323m, 3, new DateTime(2025, 10, 17, 13, 19, 32, 949, DateTimeKind.Local).AddTicks(2214) });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomersCustomerID",
                table: "Orders",
                column: "CustomersCustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomersCustomerID",
                table: "Orders",
                column: "CustomersCustomerID",
                principalTable: "Customers",
                principalColumn: "CustomerID");
        }
    }
}
