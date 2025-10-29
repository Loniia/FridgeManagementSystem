using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class RequestReturn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AllocationID",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "ReturnRequested",
                table: "FridgeAllocation",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllocationID",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ReturnRequested",
                table: "FridgeAllocation");
        }
    }
}
