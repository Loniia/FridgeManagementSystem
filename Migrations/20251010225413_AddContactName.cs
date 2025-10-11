using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddContactName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactName",
                table: "Orders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactPhone",
                table: "Orders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

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
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
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
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 11), new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9764), "Mini Fridge", 10375m, 7, new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9510) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 11), new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9791), 6213m, 2, new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9768) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 11), new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9857), "Double Door", 10997m, 8, new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9837) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 11), new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9876), "Single Door", 4812m, new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9859) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 11), new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9893), "Double Door", 5438m, 1, new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9877) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 11), new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9921), "Double Door", 6936m, 1, new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9905) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 11), new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9938), 6692m, 1, new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9923) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 11), new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9960), 10191m, 2, new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9939) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 11), new DateTime(2025, 10, 11, 0, 54, 9, 712, DateTimeKind.Local), "Mini Fridge", 5822m, new DateTime(2025, 10, 11, 0, 54, 9, 711, DateTimeKind.Local).AddTicks(9961) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 11), new DateTime(2025, 10, 11, 0, 54, 9, 712, DateTimeKind.Local).AddTicks(37), "Single Door", 5477m, 5, new DateTime(2025, 10, 11, 0, 54, 9, 712, DateTimeKind.Local).AddTicks(5) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 11), new DateTime(2025, 10, 11, 0, 54, 9, 712, DateTimeKind.Local).AddTicks(120), 11475m, 7, new DateTime(2025, 10, 11, 0, 54, 9, 712, DateTimeKind.Local).AddTicks(56) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 11), new DateTime(2025, 10, 11, 0, 54, 9, 712, DateTimeKind.Local).AddTicks(138), "Single Door", 8729m, 3, new DateTime(2025, 10, 11, 0, 54, 9, 712, DateTimeKind.Local).AddTicks(121) });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialAccounts");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropColumn(
                name: "ContactName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ContactPhone",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 10), new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6569), "Double Door", 11726m, 4, new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6398) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 10), new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6589), 11047m, 4, new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6572) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 10), new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6605), "Mini Fridge", 7960m, 2, new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6590) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 10), new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6620), "Double Door", 10754m, new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6606) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 10), new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6635), "Single Door", 10298m, 4, new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6621) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 10), new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6652), "Single Door", 4284m, 6, new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6639) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 10), new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6666), 11588m, 2, new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6654) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 10), new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6680), 9909m, 4, new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6667) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 10), new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6693), "Double Door", 8258m, new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6681) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 10), new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6712), "Mini Fridge", 11889m, 4, new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6696) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 10), new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6726), 9390m, 3, new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6713) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 10), new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6758), "Double Door", 11048m, 4, new DateTime(2025, 10, 10, 12, 1, 17, 246, DateTimeKind.Local).AddTicks(6728) });
        }
    }
}
