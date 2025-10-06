using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FridgeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class ForgetPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SecurityAnswerHash",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SecurityQuestion",
                table: "Customers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                column: "Price",
                value: 8870m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                column: "Price",
                value: 10072m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                column: "Price",
                value: 11226m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Defy Fridge 4", 7255m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5,
                column: "Price",
                value: 6532m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 6,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Hisense Fridge 6", 5247m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 7,
                columns: new[] { "Name", "Price" },
                values: new object[] { "LG Fridge 7", 6688m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 8,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Samsung Fridge 8", 5407m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 9,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Hisense Fridge 9", 6866m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 10,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Bosch Fridge 10", 4346m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 11,
                column: "Price",
                value: 9869m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 12,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Whirlpool Fridge 12", 6956m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 13,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Bosch Fridge 13", 11053m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 14,
                columns: new[] { "Name", "Price" },
                values: new object[] { "LG Fridge 14", 5525m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 15,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Bosch Fridge 15", 4880m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 16,
                columns: new[] { "Name", "Price" },
                values: new object[] { "LG Fridge 16", 7346m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 17,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Hisense Fridge 17", 6598m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 18,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Defy Fridge 18", 8257m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 19,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Hisense Fridge 19", 5346m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 20,
                column: "Price",
                value: 5877m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 21,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Bosch Fridge 21", 5392m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 22,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Bosch Fridge 22", 7392m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 23,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Whirlpool Fridge 23", 5229m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 24,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Whirlpool Fridge 24", 10696m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 25,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Whirlpool Fridge 25", 8085m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 26,
                column: "Price",
                value: 7404m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 27,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Samsung Fridge 27", 5771m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 28,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Whirlpool Fridge 28", 10484m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 29,
                columns: new[] { "Name", "Price" },
                values: new object[] { "LG Fridge 29", 11490m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 30,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Samsung Fridge 30", 6945m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecurityAnswerHash",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "SecurityQuestion",
                table: "Customers");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                column: "Price",
                value: 9586m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                column: "Price",
                value: 3807m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                column: "Price",
                value: 5394m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Bosch Fridge 4", 7106m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5,
                column: "Price",
                value: 6581m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 6,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Bosch Fridge 6", 9097m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 7,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Bosch Fridge 7", 8978m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 8,
                columns: new[] { "Name", "Price" },
                values: new object[] { "LG Fridge 8", 11223m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 9,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Defy Fridge 9", 7911m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 10,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Whirlpool Fridge 10", 7690m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 11,
                column: "Price",
                value: 4320m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 12,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Samsung Fridge 12", 3564m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 13,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Hisense Fridge 13", 7988m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 14,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Defy Fridge 14", 10109m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 15,
                columns: new[] { "Name", "Price" },
                values: new object[] { "LG Fridge 15", 6075m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 16,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Defy Fridge 16", 7436m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 17,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Whirlpool Fridge 17", 10751m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 18,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Samsung Fridge 18", 9659m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 19,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Samsung Fridge 19", 7089m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 20,
                column: "Price",
                value: 10770m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 21,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Hisense Fridge 21", 9375m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 22,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Whirlpool Fridge 22", 5580m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 23,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Hisense Fridge 23", 7893m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 24,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Bosch Fridge 24", 7028m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 25,
                columns: new[] { "Name", "Price" },
                values: new object[] { "LG Fridge 25", 9117m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 26,
                column: "Price",
                value: 10871m);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 27,
                columns: new[] { "Name", "Price" },
                values: new object[] { "LG Fridge 27", 6112m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 28,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Defy Fridge 28", 11749m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 29,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Samsung Fridge 29", 9473m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 30,
                columns: new[] { "Name", "Price" },
                values: new object[] { "Defy Fridge 30", 10000m });
        }
    }
}
