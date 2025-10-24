using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class FridgesPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7401), 9407m, 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7114) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7447), "Mini Fridge", 6818m, 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7419) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7474), 6341m, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7449) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7497), "Mini Fridge", 4413m, 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7475) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7521), 4657m, 5, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7499) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7551), "Double Door", 6980m, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7529) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7574), "Mini Fridge", 6350m, 5, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7553) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Hisense", new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7596), "Mini Fridge", 6714m, 8, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7576) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7618), "Double Door", 6907m, 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7598) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7904), 5421m, 8, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7832) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7959), 6999m, 6, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7936) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7982), "Double Door", 9363m, 3, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(7961) });

            migrationBuilder.InsertData(
                table: "Fridge",
                columns: new[] { "FridgeId", "Brand", "Condition", "CustomerID", "DateAdded", "DeliveryDate", "FaultReportId", "FridgeType", "ImageUrl", "IsActive", "LocationId", "Model", "Notes", "Price", "PurchaseDate", "Quantity", "SerialNumber", "Status", "SupplierID", "UpdatedDate", "WarrantyExpiry" },
                values: new object[,]
                {
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
                    { 25, "LG", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8421), 0, "Mini Fridge", "/images/fridges/fridge25.jpg", true, null, "Model-25", null, 9289m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8393), null },
                    { 26, "Hisense", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8443), 0, "Mini Fridge", "/images/fridges/fridge26.jpg", true, null, "Model-26", null, 4142m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8423), null },
                    { 27, "LG", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8465), 0, "Single Door", "/images/fridges/fridge27.jpg", true, null, "Model-27", null, 9067m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8445), null },
                    { 28, "Defy", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8487), 0, "Single Door", "/images/fridges/fridge28.jpg", true, null, "Model-28", null, 9860m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8467), null },
                    { 29, "Defy", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8508), 0, "Double Door", "/images/fridges/fridge29.jpg", true, null, "Model-29", null, 5150m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8488), null },
                    { 30, "LG", "Working", null, new DateOnly(2025, 10, 25), new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8556), 0, "Single Door", "/images/fridges/fridge30.jpg", true, null, "Model-30", null, 9845m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, null, "Available", 1, new DateTime(2025, 10, 25, 0, 11, 2, 200, DateTimeKind.Local).AddTicks(8510), null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 1,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 24), new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(1847), 11834m, 9, new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(1493) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 2,
                columns: new[] { "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { new DateOnly(2025, 10, 24), new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(1910), "Single Door", 6335m, 4, new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(1853) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 3,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "UpdatedDate" },
                values: new object[] { "LG", new DateOnly(2025, 10, 24), new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(1944), 6400m, new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(1912) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 4,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Samsung", new DateOnly(2025, 10, 24), new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(1968), "Single Door", 6810m, 7, new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(1946) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 5,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 24), new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(1991), 10476m, 2, new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(1970) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 6,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 24), new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(2019), "Single Door", 6450m, new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(1999) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 7,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 24), new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(2041), "Double Door", 5093m, 6, new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(2021) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 8,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 24), new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(2062), "Single Door", 6581m, 5, new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(2042) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 9,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 24), new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(2083), "Mini Fridge", 11707m, 2, new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(2063) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 10,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Defy", new DateOnly(2025, 10, 24), new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(2118), 3644m, 5, new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(2087) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 11,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 24), new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(2248), 9576m, 7, new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(2224) });

            migrationBuilder.UpdateData(
                table: "Fridge",
                keyColumn: "FridgeId",
                keyValue: 12,
                columns: new[] { "Brand", "DateAdded", "DeliveryDate", "FridgeType", "Price", "Quantity", "UpdatedDate" },
                values: new object[] { "Bosch", new DateOnly(2025, 10, 24), new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(2270), "Single Door", 7122m, 9, new DateTime(2025, 10, 24, 18, 41, 20, 868, DateTimeKind.Local).AddTicks(2250) });
        }
    }
}
