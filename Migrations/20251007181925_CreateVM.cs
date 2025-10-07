using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class CreateVM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faults_Employees_EmployeeID",
                table: "Faults");

            migrationBuilder.RenameColumn(
                name: "EmployeeID",
                table: "Faults",
                newName: "AssignedTechnicianId");

            migrationBuilder.RenameIndex(
                name: "IX_Faults_EmployeeID",
                table: "Faults",
                newName: "IX_Faults_AssignedTechnicianId");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Faults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FaultCode",
                table: "Faults",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FridgeRequests",
                columns: table => new
                {
                    FridgeRequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    RequiredModel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    RequiredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SpecialRequirements = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResponseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequestCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AdminNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FridgeRequests", x => x.FridgeRequestID);
                    table.ForeignKey(
                        name: "FK_FridgeRequests_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Hisense Fridge 1", 11218m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Samsung Fridge 2", 11877m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Bosch Fridge 3", 10295m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Defy Fridge 4", 6957m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5,
                column: "Price",
                value: 8217m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 6,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Bosch Fridge 6", 9638m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 7,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Hisense Fridge 7", 8844m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 8,
                columns: new[] { "Name", "Price" },
                values: new object[] { "LG Fridge 8", 9261m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 9,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Whirlpool Fridge 9", 6535m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 10,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Hisense Fridge 10", 4336m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 11,
                columns: new[] { "Name", "Price" },
                values: new object[] { "LG Fridge 11", 4110m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 12,
                column: "Price",
                value: 10611m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 13,
                columns: new[] { "Name", "Price" },
                values: new object[] { "LG Fridge 13", 4876m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 14,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Whirlpool Fridge 14", 8282m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 15,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Hisense Fridge 15", 7194m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 16,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Defy Fridge 16", 8473m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 17,
                column: "Price",
                value: 11805m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 18,
                column: "Price",
                value: 7756m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 19,
                columns: new[] { "Name", "Price" },
                values: new object[] { "LG Fridge 19", 9605m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 20,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Defy Fridge 20", 8323m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 21,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Hisense Fridge 21", 3559m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 22,
                columns: new[] { "Name", "Price" },
                values: new object[] { "LG Fridge 22", 7151m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 23,
                column: "Price",
                value: 5902m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 24,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Samsung Fridge 24", 3929m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 25,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Bosch Fridge 25", 7476m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 26,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Defy Fridge 26", 11999m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 27,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Bosch Fridge 27", 9285m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 28,
                column: "Price",
                value: 6078m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 29,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Hisense Fridge 29", 6773m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 30,
                column: "Price",
                value: 5779m);

            migrationBuilder.CreateIndex(
                name: "IX_Faults_CustomerId",
                table: "Faults",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_FridgeRequests_CustomerId",
                table: "FridgeRequests",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Faults_Customers_CustomerId",
                table: "Faults",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Faults_Employees_AssignedTechnicianId",
                table: "Faults",
                column: "AssignedTechnicianId",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faults_Customers_CustomerId",
                table: "Faults");

            migrationBuilder.DropForeignKey(
                name: "FK_Faults_Employees_AssignedTechnicianId",
                table: "Faults");

            migrationBuilder.DropTable(
                name: "FridgeRequests");

            migrationBuilder.DropIndex(
                name: "IX_Faults_CustomerId",
                table: "Faults");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Faults");

            migrationBuilder.DropColumn(
                name: "FaultCode",
                table: "Faults");

            migrationBuilder.RenameColumn(
                name: "AssignedTechnicianId",
                table: "Faults",
                newName: "EmployeeID");

            migrationBuilder.RenameIndex(
                name: "IX_Faults_AssignedTechnicianId",
                table: "Faults",
                newName: "IX_Faults_EmployeeID");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "Name", "Price" },
                values: new object[] { "LG Fridge 1", 9316m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Hisense Fridge 2", 7761m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Whirlpool Fridge 3", 8221m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Whirlpool Fridge 4", 8170m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5,
                column: "Price",
                value: 11481m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 6,
                columns: new[] { "Name", "Price" },
                values: new object[] { "LG Fridge 6", 6837m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 7,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Bosch Fridge 7", 8143m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 8,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Samsung Fridge 8", 4336m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 9,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Samsung Fridge 9", 8593m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 10,
                columns: new[] { "Name", "Price" },
                values: new object[] { "LG Fridge 10", 6147m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 11,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Bosch Fridge 11", 7179m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 12,
                column: "Price",
                value: 10024m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 13,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Bosch Fridge 13", 7792m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 14,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Defy Fridge 14", 5510m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 15,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Samsung Fridge 15", 6520m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 16,
                columns: new[] { "Name", "Price" },
                values: new object[] { "LG Fridge 16", 7712m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 17,
                column: "Price",
                value: 11702m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 18,
                column: "Price",
                value: 9410m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 19,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Defy Fridge 19", 5027m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 20,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Samsung Fridge 20", 8499m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 21,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Defy Fridge 21", 7801m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 22,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Hisense Fridge 22", 5099m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 23,
                column: "Price",
                value: 10094m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 24,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Whirlpool Fridge 24", 4957m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 25,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Samsung Fridge 25", 7405m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 26,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Hisense Fridge 26", 8879m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 27,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Whirlpool Fridge 27", 4514m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 28,
                column: "Price",
                value: 9443m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 29,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Samsung Fridge 29", 9303m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 30,
                column: "Price",
                value: 9016m);

            migrationBuilder.AddForeignKey(
                name: "FK_Faults_Employees_EmployeeID",
                table: "Faults",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }
    }
}
