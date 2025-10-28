using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixSupplierColumnMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotations_Suppliers_SupplierId",
                table: "Quotations");

            migrationBuilder.DropColumn(
                name: "DeliveryTimeframe",
                table: "Quotations");

            migrationBuilder.DropColumn(
                name: "TermsAndConditions",
                table: "Quotations");

            migrationBuilder.DropColumn(
                name: "ValidUntil",
                table: "Quotations");

            migrationBuilder.RenameColumn(
                name: "SupplierId",
                table: "Quotations",
                newName: "SupplierID");

            migrationBuilder.RenameIndex(
                name: "IX_Quotations_SupplierId",
                table: "Quotations",
                newName: "IX_Quotations_SupplierID");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotations_Suppliers_SupplierID",
                table: "Quotations",
                column: "SupplierID",
                principalTable: "Suppliers",
                principalColumn: "SupplierID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotations_Suppliers_SupplierID",
                table: "Quotations");

            migrationBuilder.RenameColumn(
                name: "SupplierID",
                table: "Quotations",
                newName: "SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Quotations_SupplierID",
                table: "Quotations",
                newName: "IX_Quotations_SupplierId");

            migrationBuilder.AddColumn<int>(
                name: "DeliveryTimeframe",
                table: "Quotations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TermsAndConditions",
                table: "Quotations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidUntil",
                table: "Quotations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Quotations_Suppliers_SupplierId",
                table: "Quotations",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "SupplierID");
        }
    }
}
