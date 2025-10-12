using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class RemovedCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialAccounts");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerID",
                table: "PurchaseRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2111), 10814m, 5, new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(1870) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2142), "Single Door", 11746m, 4, new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2117) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DeliveryDate", "Price", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2164), 4169m, new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2144) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2185), "Double Door", 11274m, 8, new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2166) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2207), "Mini Fridge", 11281m, new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2187) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2234), "Mini Fridge", 9679m, new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2213) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2254), 9170m, 6, new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2236) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2275), 5955m, 8, new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2256) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2306), "Single Door", 6521m, 5, new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2277) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2350), 9439m, 1, new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2311) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2402), "Double Door", 9209m, 4, new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2380) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2423), "Mini Fridge", 4331m, new DateTime(2025, 10, 11, 22, 17, 47, 976, DateTimeKind.Local).AddTicks(2403) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CustomerID",
                table: "PurchaseRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "FinancialAccounts",
                columns: table => new
                {
                    FinancialAccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialAccounts", x => x.FinancialAccountId);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transactions_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK_Transactions_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId");
                    table.ForeignKey(
                        name: "FK_Transactions_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "PaymentId");
                });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9764), 10375m, 7, new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9510) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9791), "Mini Fridge", 6213m, 2, new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9768) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DeliveryDate", "Price", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9857), 10997m, new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9837) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9876), "Single Door", 4812m, 1, new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9859) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9893), "Double Door", 5438m, new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9877) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9921), "Double Door", 6936m, new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9905) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9938), 6692m, 1, new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9923) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9960), 10191m, 2, new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9939) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 11, 0, 54, 9, 712, DateTimeKind.Local), "Mini Fridge", 5822m, 6, new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9961) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 11, 0, 54, 9, 712, DateTimeKind.Local).AddTicks(37), 5477m, 5, new DateTime(2025, 10, 11, 0, 54, 9, 712, DateTimeKind.Local).AddTicks(5) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 11, 0, 54, 9, 712, DateTimeKind.Local).AddTicks(120), "Mini Fridge", 11475m, 7, new DateTime(2025, 10, 11, 0, 54, 9, 712, DateTimeKind.Local).AddTicks(56) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 11, 0, 54, 9, 712, DateTimeKind.Local).AddTicks(138), "Single Door", 8729m, new DateTime(2025, 10, 11, 0, 54, 9, 712, DateTimeKind.Local).AddTicks(121) });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CustomerID",
                table: "Transactions",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_OrderId",
                table: "Transactions",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PaymentId",
                table: "Transactions",
                column: "PaymentId");
        }
    }
}
