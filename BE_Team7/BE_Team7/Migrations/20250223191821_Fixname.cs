using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BE_Team7.Migrations
{
    /// <inheritdoc />
    public partial class Fixname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5635d7a1-72e9-47b4-9366-9dd7c09676ba");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c9b5bcf7-6213-43a3-b306-52a02a447037");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cca9d37f-93fe-4609-a234-5625532d13ad");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Category",
                newName: "CategoryName");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "057f6942-c854-4dbb-b584-dde28c29eeb0", null, "User", "USER" },
                    { "29032632-bff4-4282-bc03-327ca32ddd47", null, "Staff", "STAFF" },
                    { "f8107162-572c-452e-bdae-0112050d692e", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "057f6942-c854-4dbb-b584-dde28c29eeb0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "29032632-bff4-4282-bc03-327ca32ddd47");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f8107162-572c-452e-bdae-0112050d692e");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "Category",
                newName: "Name");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5635d7a1-72e9-47b4-9366-9dd7c09676ba", null, "Staff", "STAFF" },
                    { "c9b5bcf7-6213-43a3-b306-52a02a447037", null, "Admin", "ADMIN" },
                    { "cca9d37f-93fe-4609-a234-5625532d13ad", null, "User", "USER" }
                });
        }
    }
}
