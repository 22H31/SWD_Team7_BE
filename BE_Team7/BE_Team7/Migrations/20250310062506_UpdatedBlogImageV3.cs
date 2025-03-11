using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BE_Team7.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedBlogImageV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3d7d8c86-45cc-4165-853b-1603d38339ca");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3ebd106b-3782-4f54-aa2d-a65db8eb4e59");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a49c4dc9-5ffe-4a81-8915-dfd8feaecd76");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f3ca812b-6137-46cd-9364-e2e473805756");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4e692e81-593c-4cd0-83ba-77f4c8699af7", null, "Admin", "ADMIN" },
                    { "617fb802-00c8-4af1-ae47-f72cf81416f5", null, "StaffSale", "STAFFSALE" },
                    { "8b1b6a7f-35de-4482-9f8d-c21f83f36494", null, "Staff", "STAFF" },
                    { "a066de1f-7fb1-43dd-b33b-6ac6384bc944", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e692e81-593c-4cd0-83ba-77f4c8699af7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "617fb802-00c8-4af1-ae47-f72cf81416f5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8b1b6a7f-35de-4482-9f8d-c21f83f36494");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a066de1f-7fb1-43dd-b33b-6ac6384bc944");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3d7d8c86-45cc-4165-853b-1603d38339ca", null, "StaffSale", "STAFFSALE" },
                    { "3ebd106b-3782-4f54-aa2d-a65db8eb4e59", null, "Staff", "STAFF" },
                    { "a49c4dc9-5ffe-4a81-8915-dfd8feaecd76", null, "User", "USER" },
                    { "f3ca812b-6137-46cd-9364-e2e473805756", null, "Admin", "ADMIN" }
                });
        }
    }
}
