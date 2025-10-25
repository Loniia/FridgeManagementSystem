using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdatefaultReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 30);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "FaultReport",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "FaultReport");

            migrationBuilder.InsertData(
                table: "Fridge",
                columns: new[] { "FridgeId", "Brand", "Condition", "CustomerID", "DateAdded", "DeliveryDate", "FaultReportId", "FridgeType", "ImageUrl", "IsActive", "LocationId", "Model", "Notes", "Price", "PurchaseDate", "Quantity", "SerialNumber", "Status", "SupplierID", "UpdatedDate", "WarrantyExpiry" },
                values: new object[,]
                {
                    { 1, "Defy", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7401), 0, "Single Door", "/images/fridges/fridge1.jpg", true, null, "Model-1", null, 9407m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7114), null },
                    { 2, "LG", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7447), 0, "Mini Fridge", "/images/fridges/fridge2.jpg", true, null, "Model-2", null, 6818m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7419), null },
                    { 3, "Defy", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7474), 0, "Single Door", "/images/fridges/fridge3.jpg", true, null, "Model-3", null, 6341m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7449), null },
                    { 4, "Defy", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7497), 0, "Mini Fridge", "/images/fridges/fridge4.jpg", true, null, "Model-4", null, 4413m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7475), null },
                    { 5, "Hisense", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7521), 0, "Single Door", "/images/fridges/fridge5.jpg", true, null, "Model-5", null, 4657m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7499), null },
                    { 6, "Samsung", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7551), 0, "Double Door", "/images/fridges/fridge6.jpg", true, null, "Model-6", null, 6980m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7529), null },
                    { 7, "Hisense", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7574), 0, "Mini Fridge", "/images/fridges/fridge7.jpg", true, null, "Model-7", null, 6350m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7553), null },
                    { 8, "Hisense", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7596), 0, "Mini Fridge", "/images/fridges/fridge8.jpg", true, null, "Model-8", null, 6714m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7576), null },
                    { 9, "Samsung", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7618), 0, "Double Door", "/images/fridges/fridge9.jpg", true, null, "Model-9", null, 6907m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7598), null },
                    { 10, "Bosch", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7904), 0, "Double Door", "/images/fridges/fridge10.jpg", true, null, "Model-10", null, 5421m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7832), null },
                    { 11, "Defy", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7959), 0, "Mini Fridge", "/images/fridges/fridge11.jpg", true, null, "Model-11", null, 6999m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7936), null },
                    { 12, "Samsung", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7982), 0, "Double Door", "/images/fridges/fridge12.jpg", true, null, "Model-12", null, 9363m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7961), null },
                    { 13, "LG", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8004), 0, "Double Door", "/images/fridges/fridge13.jpg", true, null, "Model-13", null, 9028m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7983), null },
                    { 14, "Hisense", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8025), 0, "Double Door", "/images/fridges/fridge14.jpg", true, null, "Model-14", null, 5199m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8005), null },
                    { 15, "LG", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8076), 0, "Double Door", "/images/fridges/fridge15.jpg", true, null, "Model-15", null, 8555m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8027), null },
                    { 16, "Defy", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8099), 0, "Mini Fridge", "/images/fridges/fridge16.jpg", true, null, "Model-16", null, 3549m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8078), null },
                    { 17, "Hisense", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8120), 0, "Double Door", "/images/fridges/fridge17.jpg", true, null, "Model-17", null, 10703m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8101), null },
                    { 18, "Defy", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8147), 0, "Double Door", "/images/fridges/fridge18.jpg", true, null, "Model-18", null, 11398m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8126), null },
                    { 19, "Hisense", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8169), 0, "Mini Fridge", "/images/fridges/fridge19.jpg", true, null, "Model-19", null, 5751m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8149), null },
                    { 20, "Hisense", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8191), 0, "Double Door", "/images/fridges/fridge20.jpg", true, null, "Model-20", null, 6006m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8172), null },
                    { 21, "Bosch", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8212), 0, "Mini Fridge", "/images/fridges/fridge21.jpg", true, null, "Model-21", null, 10154m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8193), null },
                    { 22, "Defy", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8234), 0, "Double Door", "/images/fridges/fridge22.jpg", true, null, "Model-22", null, 9945m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8214), null },
                    { 23, "Bosch", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8255), 0, "Mini Fridge", "/images/fridges/fridge23.jpg", true, null, "Model-23", null, 9279m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8236), null },
                    { 24, "Defy", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8277), 0, "Double Door", "/images/fridges/fridge24.jpg", true, null, "Model-24", null, 7507m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8257), null },
                    { 25, "LG", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8421), 0, "Mini Fridge", "/images/fridges/fridge25.jpg", true, null, "Model-25", null, 9289m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8393), null },
                    { 26, "Hisense", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8443), 0, "Mini Fridge", "/images/fridges/fridge26.jpg", true, null, "Model-26", null, 4142m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8423), null },
                    { 27, "LG", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8465), 0, "Single Door", "/images/fridges/fridge27.jpg", true, null, "Model-27", null, 9067m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8445), null },
                    { 28, "Defy", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8487), 0, "Single Door", "/images/fridges/fridge28.jpg", true, null, "Model-28", null, 9860m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8467), null },
                    { 29, "Defy", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8508), 0, "Double Door", "/images/fridges/fridge29.jpg", true, null, "Model-29", null, 5150m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8488), null },
                    { 30, "LG", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8556), 0, "Single Door", "/images/fridges/fridge30.jpg", true, null, "Model-30", null, 9845m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8510), null }
                });
        }
    }
}
