using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class Payment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialAccounts");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PayPalTransactionId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProofOfPaymentPath",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceCode",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8224), "Single Door", 5945m, 5, new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(7735) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8253), 9876m, 8, new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8231) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8296), "Mini Fridge", 9049m, 5, new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8255) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8317), "Mini Fridge", 7454m, 7, new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8298) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8336), "Single Door", 6157m, 5, new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8318) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8363), "Single Door", 5566m, 9, new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8344) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8408), "Single Door", 8566m, new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8365) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8430), 7994m, 1, new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8410) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8451), 6211m, 7, new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8432) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8494), "Mini Fridge", 4722m, new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8456) });

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
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8561), "Mini Fridge", 4846m, 6, new DateTime(2025, 10, 11, 20, 22, 8, 625, DateTimeKind.Local).AddTicks(8543) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PayPalTransactionId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ProofOfPaymentPath",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ReferenceCode",
                table: "Payments");

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
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9764), "Mini Fridge", 10375m, 7, new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9510) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9791), 6213m, 2, new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9768) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9857), "Double Door", 10997m, 8, new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9837) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9876), "Single Door", 4812m, 1, new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9859) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9893), "Double Door", 5438m, 1, new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9877) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9921), "Double Door", 6936m, 1, new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9905) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9938), "Double Door", 6692m, new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9923) });

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
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateTime(2025, 10, 11, 0, 54, 9, 712, DateTimeKind.Local), 5822m, 6, new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9961) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Hisense", new DateTime(2025, 10, 11, 0, 54, 9, 712, DateTimeKind.Local).AddTicks(37), "Single Door", 5477m, new DateTime(2025, 10, 11, 0, 54, 9, 712, DateTimeKind.Local).AddTicks(5) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateTime(2025, 10, 11, 0, 54, 9, 712, DateTimeKind.Local).AddTicks(120), 11475m, 7, new DateTime(2025, 10, 11, 0, 54, 9, 712, DateTimeKind.Local).AddTicks(56) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateTime(2025, 10, 11, 0, 54, 9, 712, DateTimeKind.Local).AddTicks(138), "Single Door", 8729m, 3, new DateTime(2025, 10, 11, 0, 54, 9, 712, DateTimeKind.Local).AddTicks(121) });

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
