using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BE_Team7.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3d921f3-6586-4134-ba78-47fc88fd90ff");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d6222661-dc5d-4461-844a-5d4de62993d3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dc8dcc22-7435-4bab-ad8b-c0eace3d7189");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ea6e86b3-ced5-4428-8a88-93bbe5c4d88d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "ad18d7c5-aafb-475e-9c2d-e7c3986e42d5", null, "Admin", "ADMIN" },
                    { "c2be16b1-1f02-4014-8c29-10d47226abf6", null, "StaffSale", "STAFFSALE" },
                    { "e46ba7b7-565c-4459-821c-c6f2cd7a783f", null, "Staff", "STAFF" },
                    { "f436e063-99a7-4453-8475-d5b0dea5ac14", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ad18d7c5-aafb-475e-9c2d-e7c3986e42d5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c2be16b1-1f02-4014-8c29-10d47226abf6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e46ba7b7-565c-4459-821c-c6f2cd7a783f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f436e063-99a7-4453-8475-d5b0dea5ac14");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b3d921f3-6586-4134-ba78-47fc88fd90ff", null, "Staff", "STAFF" },
                    { "d6222661-dc5d-4461-844a-5d4de62993d3", null, "StaffSale", "STAFFSALE" },
                    { "dc8dcc22-7435-4bab-ad8b-c0eace3d7189", null, "User", "USER" },
                    { "ea6e86b3-ced5-4428-8a88-93bbe5c4d88d", null, "Admin", "ADMIN" }
                });
        }
    }
}
