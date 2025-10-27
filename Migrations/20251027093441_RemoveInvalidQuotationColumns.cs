using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class RemoveInvalidQuotationColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupplierID",
                table: "RequestsForQuotation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Quotations",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_RequestsForQuotation_SupplierID",
                table: "RequestsForQuotation",
                column: "SupplierID");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestsForQuotation_Suppliers_SupplierID",
                table: "RequestsForQuotation",
                column: "SupplierID",
                principalTable: "Suppliers",
                principalColumn: "SupplierID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestsForQuotation_Suppliers_SupplierID",
                table: "RequestsForQuotation");

            migrationBuilder.DropIndex(
                name: "IX_RequestsForQuotation_SupplierID",
                table: "RequestsForQuotation");

            migrationBuilder.DropColumn(
                name: "SupplierID",
                table: "RequestsForQuotation");

            migrationBuilder.DropColumn(
                name: "DeliveryTimeframe",
                table: "Quotations");

            migrationBuilder.DropColumn(
                name: "TermsAndConditions",
                table: "Quotations");

            migrationBuilder.DropColumn(
                name: "ValidUntil",
                table: "Quotations");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Quotations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);
        }
    }
}
