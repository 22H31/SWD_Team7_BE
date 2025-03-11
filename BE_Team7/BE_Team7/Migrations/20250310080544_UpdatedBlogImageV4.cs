using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BE_Team7.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedBlogImageV4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "SubTitle",
                table: "Blog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "15b905b2-7715-4735-9c4e-e55647ed289e", null, "Staff", "STAFF" },
                    { "54349ced-c4b9-4476-a7a2-06a7cf2902e5", null, "Admin", "ADMIN" },
                    { "8969ac71-6374-4a5f-810b-3a15d2d06dfd", null, "StaffSale", "STAFFSALE" },
                    { "b7438316-ef97-4e6d-aa74-1bf1370147f5", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "15b905b2-7715-4735-9c4e-e55647ed289e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "54349ced-c4b9-4476-a7a2-06a7cf2902e5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8969ac71-6374-4a5f-810b-3a15d2d06dfd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b7438316-ef97-4e6d-aa74-1bf1370147f5");

            migrationBuilder.DropColumn(
                name: "SubTitle",
                table: "Blog");

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
    }
}
