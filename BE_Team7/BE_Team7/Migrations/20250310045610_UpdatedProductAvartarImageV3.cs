using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BE_Team7.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedProductAvartarImageV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "14c0ae17-9d30-456c-961b-ed2708aee7d7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "21eb0274-e668-4c72-ba9b-c1fbb3566cc3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "75e7b7c6-5c2d-451b-8e16-6e95009332c4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c9c63331-b8d9-4b23-962b-298569024b60");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "971ca256-49a9-44a0-8693-5cd32109195c", null, "StaffSale", "STAFFSALE" },
                    { "c8872f88-7605-439d-b020-d7cfae08559a", null, "User", "USER" },
                    { "e33d5066-1628-49fd-b140-18fea9afccad", null, "Staff", "STAFF" },
                    { "f30dcf86-3b3d-4f68-b1c3-d0c5af2a7057", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "971ca256-49a9-44a0-8693-5cd32109195c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c8872f88-7605-439d-b020-d7cfae08559a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e33d5066-1628-49fd-b140-18fea9afccad");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f30dcf86-3b3d-4f68-b1c3-d0c5af2a7057");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "14c0ae17-9d30-456c-961b-ed2708aee7d7", null, "Staff", "STAFF" },
                    { "21eb0274-e668-4c72-ba9b-c1fbb3566cc3", null, "Admin", "ADMIN" },
                    { "75e7b7c6-5c2d-451b-8e16-6e95009332c4", null, "User", "USER" },
                    { "c9c63331-b8d9-4b23-962b-298569024b60", null, "StaffSale", "STAFFSALE" }
                });
        }
    }
}
