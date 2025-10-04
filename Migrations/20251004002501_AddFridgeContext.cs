using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddFridgeContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Inventory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_ProductId",
                table: "Inventory",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Products_ProductId",
                table: "Inventory",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Products_ProductId",
                table: "Inventory");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_ProductId",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Inventory");
        }
    }
}
