using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BE_Team7.Migrations
{
    /// <inheritdoc />
    public partial class FixProductV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "04d997ca-cdd9-458f-84bb-eefc7dfe4261");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2a710d76-8a15-42ac-bb13-22b9f0ae5675");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "62a795e0-42e3-463c-82eb-57fe4d7b5072");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ea5d6dbe-c8ab-429c-ae41-9aacd2bdadcc");

            migrationBuilder.AddColumn<int>(
                name: "SoldQuantity",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "04011ca5-ee99-400b-90e1-2bc0ab49fb43", null, "StaffSale", "STAFFSALE" },
                    { "a3162081-fdb1-4818-b6d5-e8088943d624", null, "Staff", "STAFF" },
                    { "af0de568-05ec-45a0-8e92-2e1fd25fa56f", null, "User", "USER" },
                    { "dc153b60-5932-444a-b70d-9e03ea235817", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "04011ca5-ee99-400b-90e1-2bc0ab49fb43");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3162081-fdb1-4818-b6d5-e8088943d624");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "af0de568-05ec-45a0-8e92-2e1fd25fa56f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dc153b60-5932-444a-b70d-9e03ea235817");

            migrationBuilder.DropColumn(
                name: "SoldQuantity",
                table: "Products");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "04d997ca-cdd9-458f-84bb-eefc7dfe4261", null, "User", "USER" },
                    { "2a710d76-8a15-42ac-bb13-22b9f0ae5675", null, "StaffSale", "STAFFSALE" },
                    { "62a795e0-42e3-463c-82eb-57fe4d7b5072", null, "Admin", "ADMIN" },
                    { "ea5d6dbe-c8ab-429c-ae41-9aacd2bdadcc", null, "Staff", "STAFF" }
                });
        }
    }
}
