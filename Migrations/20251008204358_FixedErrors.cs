using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixedErrors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Carts_CartId1",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Customers_CustomerId",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Fridge_Customers_CustomerId",
                table: "Fridge");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Products_ProductId",
                table: "Inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Products_OrderId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Products_ProductId",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_ProductId",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Inventory");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Reviews",
                newName: "FridgeId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ProductId",
                table: "Reviews",
                newName: "IX_Reviews_FridgeId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Orders",
                newName: "CustomerID");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                newName: "IX_Orders_CustomerID");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "OrderItems",
                newName: "FridgeId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Fridge",
                newName: "CustomerID");

            migrationBuilder.RenameIndex(
                name: "IX_Fridge_CustomerId",
                table: "Fridge",
                newName: "IX_Fridge_CustomerID");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Carts",
                newName: "CustomerID");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_CustomerId",
                table: "Carts",
                newName: "IX_Carts_CustomerID");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "CartItems",
                newName: "FridgeId");

            migrationBuilder.RenameColumn(
                name: "CartId1",
                table: "CartItems",
                newName: "CartsCartId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                newName: "IX_CartItems_FridgeId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_CartId1",
                table: "CartItems",
                newName: "IX_CartItems_CartsCartId");

            migrationBuilder.AlterColumn<string>(
                name: "ContactPerson",
                table: "Suppliers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "OrdersOrderId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomersCustomerID",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrdersOrderId",
                table: "OrderItems",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CartId",
                table: "CartItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "CartItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "SupplierID", "Address", "ContactPerson", "Email", "FridgeId", "IsActive", "Name", "Phone", "PurchaseOrderID", "QuotationID" },
                values: new object[] { 1, "123 Main Street", null, "supplier@example.com", 0, true, "Default Supplier", "0123456789", 0, 0 });

            migrationBuilder.InsertData(
                table: "Fridge",
                columns: new[] { "FridgeId", "Brand", "Condition", "CustomerID", "DateAdded", "DeliveryDate", "FaultID", "FridgeType", "ImageUrl", "IsActive", "LocationId", "Model", "Price", "PurchaseDate", "Quantity", "SerialNumber", "Status", "SupplierID", "UpdatedDate", "WarrantyExpiry" },
                values: new object[,]
                {
                    { 1, "LG", "Working", null, new DateOnly(2025, 10, 8), new DateTime(2025, 10, 8, 22, 43, 52, 996, DateTimeKind.Local).AddTicks(9212), 0, "Single Door", "/images/fridges/fridge1.jpg", true, null, "Model-1", 5651m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "Available", 1, new DateTime(2025, 10, 8, 22, 43, 52, 996, DateTimeKind.Local).AddTicks(8989), null },
                    { 2, "LG", "Working", null, new DateOnly(2025, 10, 8), new DateTime(2025, 10, 8, 22, 43, 52, 996, DateTimeKind.Local).AddTicks(9276), 0, "Mini Fridge", "/images/fridges/fridge2.jpg", true, null, "Model-2", 11035m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, null, "Available", 1, new DateTime(2025, 10, 8, 22, 43, 52, 996, DateTimeKind.Local).AddTicks(9233), null },
                    { 3, "LG", "Working", null, new DateOnly(2025, 10, 8), new DateTime(2025, 10, 8, 22, 43, 52, 996, DateTimeKind.Local).AddTicks(9310), 0, "Double Door", "/images/fridges/fridge3.jpg", true, null, "Model-3", 8917m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, null, "Available", 1, new DateTime(2025, 10, 8, 22, 43, 52, 996, DateTimeKind.Local).AddTicks(9278), null },
                    { 4, "Hisense", "Working", null, new DateOnly(2025, 10, 8), new DateTime(2025, 10, 8, 22, 43, 52, 996, DateTimeKind.Local).AddTicks(9333), 0, "Single Door", "/images/fridges/fridge4.jpg", true, null, "Model-4", 4326m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, null, "Available", 1, new DateTime(2025, 10, 8, 22, 43, 52, 996, DateTimeKind.Local).AddTicks(9312), null },
                    { 5, "Samsung", "Working", null, new DateOnly(2025, 10, 8), new DateTime(2025, 10, 8, 22, 43, 52, 996, DateTimeKind.Local).AddTicks(9356), 0, "Single Door", "/images/fridges/fridge5.jpg", true, null, "Model-5", 11373m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, null, "Available", 1, new DateTime(2025, 10, 8, 22, 43, 52, 996, DateTimeKind.Local).AddTicks(9335), null },
                    { 6, "Defy", "Working", null, new DateOnly(2025, 10, 8), new DateTime(2025, 10, 8, 22, 43, 52, 996, DateTimeKind.Local).AddTicks(9383), 0, "Mini Fridge", "/images/fridges/fridge6.jpg", true, null, "Model-6", 9887m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, null, "Available", 1, new DateTime(2025, 10, 8, 22, 43, 52, 996, DateTimeKind.Local).AddTicks(9362), null },
                    { 7, "LG", "Working", null, new DateOnly(2025, 10, 8), new DateTime(2025, 10, 8, 22, 43, 52, 996, DateTimeKind.Local).AddTicks(9404), 0, "Double Door", "/images/fridges/fridge7.jpg", true, null, "Model-7", 8280m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "Available", 1, new DateTime(2025, 10, 8, 22, 43, 52, 996, DateTimeKind.Local).AddTicks(9384), null },
                    { 8, "LG", "Working", null, new DateOnly(2025, 10, 8), new DateTime(2025, 10, 8, 22, 43, 52, 996, DateTimeKind.Local).AddTicks(9425), 0, "Mini Fridge", "/images/fridges/fridge8.jpg", true, null, "Model-8", 8658m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, null, "Available", 1, new DateTime(2025, 10, 8, 22, 43, 52, 996, DateTimeKind.Local).AddTicks(9405), null },
                    { 9, "Bosch", "Working", null, new DateOnly(2025, 10, 8), new DateTime(2025, 10, 8, 22, 43, 52, 996, DateTimeKind.Local).AddTicks(9444), 0, "Mini Fridge", "/images/fridges/fridge9.jpg", true, null, "Model-9", 5933m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, null, "Available", 1, new DateTime(2025, 10, 8, 22, 43, 52, 996, DateTimeKind.Local).AddTicks(9426), null },
                    { 10, "Bosch", "Working", null, new DateOnly(2025, 10, 8), new DateTime(2025, 10, 8, 22, 43, 52, 996, DateTimeKind.Local).AddTicks(9477), 0, "Double Door", "/images/fridges/fridge10.jpg", true, null, "Model-10", 9426m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, null, "Available", 1, new DateTime(2025, 10, 8, 22, 43, 52, 996, DateTimeKind.Local).AddTicks(9448), null },
                    { 11, "Bosch", "Working", null, new DateOnly(2025, 10, 8), new DateTime(2025, 10, 8, 22, 43, 52, 996, DateTimeKind.Local).AddTicks(9522), 0, "Double Door", "/images/fridges/fridge11.jpg", true, null, "Model-11", 6550m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, null, "Available", 1, new DateTime(2025, 10, 8, 22, 43, 52, 996, DateTimeKind.Local).AddTicks(9500), null },
                    { 12, "Samsung", "Working", null, new DateOnly(2025, 10, 8), new DateTime(2025, 10, 8, 22, 43, 52, 996, DateTimeKind.Local).AddTicks(9542), 0, "Single Door", "/images/fridges/fridge12.jpg", true, null, "Model-12", 3819m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Available", 1, new DateTime(2025, 10, 8, 22, 43, 52, 996, DateTimeKind.Local).AddTicks(9523), null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrdersOrderId",
                table: "Payments",
                column: "OrdersOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomersCustomerID",
                table: "Orders",
                column: "CustomersCustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_FridgeId",
                table: "OrderItems",
                column: "FridgeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrdersOrderId",
                table: "OrderItems",
                column: "OrdersOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_CartsCartId",
                table: "CartItems",
                column: "CartsCartId",
                principalTable: "Carts",
                principalColumn: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Fridge_FridgeId",
                table: "CartItems",
                column: "FridgeId",
                principalTable: "Fridge",
                principalColumn: "FridgeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Customers_CustomerID",
                table: "Carts",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Fridge_Customers_CustomerID",
                table: "Fridge",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Fridge_FridgeId",
                table: "OrderItems",
                column: "FridgeId",
                principalTable: "Fridge",
                principalColumn: "FridgeId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrdersOrderId",
                table: "OrderItems",
                column: "OrdersOrderId",
                principalTable: "Orders",
                principalColumn: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerID",
                table: "Orders",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomersCustomerID",
                table: "Orders",
                column: "CustomersCustomerID",
                principalTable: "Customers",
                principalColumn: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Orders_OrdersOrderId",
                table: "Payments",
                column: "OrdersOrderId",
                principalTable: "Orders",
                principalColumn: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Fridge_FridgeId",
                table: "Reviews",
                column: "FridgeId",
                principalTable: "Fridge",
                principalColumn: "FridgeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Carts_CartsCartId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Fridge_FridgeId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Customers_CustomerID",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Fridge_Customers_CustomerID",
                table: "Fridge");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Fridge_FridgeId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrdersOrderId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomersCustomerID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Orders_OrdersOrderId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Fridge_FridgeId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Payments_OrdersOrderId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomersCustomerID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_FridgeId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_OrdersOrderId",
                table: "OrderItems");

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "SupplierID",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "OrdersOrderId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CustomersCustomerID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrdersOrderId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "FridgeId",
                table: "Reviews",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_FridgeId",
                table: "Reviews",
                newName: "IX_Reviews_ProductId");

            migrationBuilder.RenameColumn(
                name: "CustomerID",
                table: "Orders",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CustomerID",
                table: "Orders",
                newName: "IX_Orders_CustomerId");

            migrationBuilder.RenameColumn(
                name: "FridgeId",
                table: "OrderItems",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "CustomerID",
                table: "Fridge",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Fridge_CustomerID",
                table: "Fridge",
                newName: "IX_Fridge_CustomerId");

            migrationBuilder.RenameColumn(
                name: "CustomerID",
                table: "Carts",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_CustomerID",
                table: "Carts",
                newName: "IX_Carts_CustomerId");

            migrationBuilder.RenameColumn(
                name: "FridgeId",
                table: "CartItems",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "CartsCartId",
                table: "CartItems",
                newName: "CartId1");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_FridgeId",
                table: "CartItems",
                newName: "IX_CartItems_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_CartsCartId",
                table: "CartItems",
                newName: "IX_CartItems_CartId1");

            migrationBuilder.AlterColumn<string>(
                name: "ContactPerson",
                table: "Suppliers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Inventory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CartId",
                table: "CartItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[] { 1, "Fridges" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CategoryId", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge1.jpg", "Hisense Fridge 1", 11218m },
                    { 2, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge2.jpg", "Samsung Fridge 2", 11877m },
                    { 3, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge3.jpg", "Bosch Fridge 3", 10295m },
                    { 4, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge4.jpg", "Defy Fridge 4", 6957m },
                    { 5, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge5.jpg", "Whirlpool Fridge 5", 8217m },
                    { 6, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge6.jpg", "Bosch Fridge 6", 9638m },
                    { 7, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge7.jpg", "Hisense Fridge 7", 8844m },
                    { 8, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge8.jpg", "LG Fridge 8", 9261m },
                    { 9, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge9.jpg", "Whirlpool Fridge 9", 6535m },
                    { 10, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge10.jpg", "Hisense Fridge 10", 4336m },
                    { 11, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge11.jpg", "LG Fridge 11", 4110m },
                    { 12, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge12.jpg", "Defy Fridge 12", 10611m },
                    { 13, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge13.jpg", "LG Fridge 13", 4876m },
                    { 14, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge14.jpg", "Whirlpool Fridge 14", 8282m },
                    { 15, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge15.jpg", "Hisense Fridge 15", 7194m },
                    { 16, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge16.jpg", "Defy Fridge 16", 8473m },
                    { 17, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge17.jpg", "Hisense Fridge 17", 11805m },
                    { 18, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge18.jpg", "Defy Fridge 18", 7756m },
                    { 19, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge19.jpg", "LG Fridge 19", 9605m },
                    { 20, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge20.jpg", "Defy Fridge 20", 8323m },
                    { 21, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge21.jpg", "Hisense Fridge 21", 3559m },
                    { 22, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge22.jpg", "LG Fridge 22", 7151m },
                    { 23, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge23.jpg", "Bosch Fridge 23", 5902m },
                    { 24, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge24.jpg", "Samsung Fridge 24", 3929m },
                    { 25, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge25.jpg", "Bosch Fridge 25", 7476m },
                    { 26, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge26.jpg", "Defy Fridge 26", 11999m },
                    { 27, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge27.jpg", "Bosch Fridge 27", 9285m },
                    { 28, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge28.jpg", "Samsung Fridge 28", 6078m },
                    { 29, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge29.jpg", "Hisense Fridge 29", 6773m },
                    { 30, 1, "High quality and energy-efficient fridge suitable for all households.", "fridge30.jpg", "Whirlpool Fridge 30", 5779m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_ProductId",
                table: "Inventory",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_CartId1",
                table: "CartItems",
                column: "CartId1",
                principalTable: "Carts",
                principalColumn: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Customers_CustomerId",
                table: "Carts",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Fridge_Customers_CustomerId",
                table: "Fridge",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Products_ProductId",
                table: "Inventory",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Products_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Products",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Products_ProductId",
                table: "Reviews",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");
        }
    }
}
