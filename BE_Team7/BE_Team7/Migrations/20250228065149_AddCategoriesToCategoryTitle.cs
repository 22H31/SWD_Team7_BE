using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BE_Team7.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoriesToCategoryTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c6607a38-9d89-4479-b395-7bb9980f7f45");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d04eaa2c-0ac0-409c-bfbd-558ce662d2ec");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f0668276-427a-493c-bb06-0685b96dfb3c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "92dde644-c5f9-4a2e-a37d-32509ee2de09", null, "Staff", "STAFF" },
                    { "c196fd02-50e6-4e02-93f3-0e36371060d5", null, "Admin", "ADMIN" },
                    { "cd384879-e294-4dc6-b09f-130c8e62e297", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "92dde644-c5f9-4a2e-a37d-32509ee2de09");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c196fd02-50e6-4e02-93f3-0e36371060d5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cd384879-e294-4dc6-b09f-130c8e62e297");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "c6607a38-9d89-4479-b395-7bb9980f7f45", null, "Admin", "ADMIN" },
                    { "d04eaa2c-0ac0-409c-bfbd-558ce662d2ec", null, "Staff", "STAFF" },
                    { "f0668276-427a-493c-bb06-0685b96dfb3c", null, "User", "USER" }
                });
        }
    }
}
