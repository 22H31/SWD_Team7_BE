using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BE_Team7.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserNameFromFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aeb29d51-e024-4566-b56c-df52db717443");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b2987919-206c-4254-85c8-fa06821ed289");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b478b0a7-d3c4-47ab-b7ef-e633bd8a5a4f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f1a36279-d8bb-4f9c-8fce-32119e87a670");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Feedback");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "583e40be-648e-4bb8-9fc1-c6cd981200b0", null, "StaffSale", "STAFFSALE" },
                    { "83180f8f-5c9c-4e9b-bee9-b4d772eb6145", null, "User", "USER" },
                    { "9010ca70-db97-4a8a-9b21-b6f96c51529f", null, "Admin", "ADMIN" },
                    { "e73d6a19-526b-4cd5-be31-69a31fec18b3", null, "Staff", "STAFF" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "583e40be-648e-4bb8-9fc1-c6cd981200b0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "83180f8f-5c9c-4e9b-bee9-b4d772eb6145");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9010ca70-db97-4a8a-9b21-b6f96c51529f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e73d6a19-526b-4cd5-be31-69a31fec18b3");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Feedback",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "aeb29d51-e024-4566-b56c-df52db717443", null, "StaffSale", "STAFFSALE" },
                    { "b2987919-206c-4254-85c8-fa06821ed289", null, "User", "USER" },
                    { "b478b0a7-d3c4-47ab-b7ef-e633bd8a5a4f", null, "Staff", "STAFF" },
                    { "f1a36279-d8bb-4f9c-8fce-32119e87a670", null, "Admin", "ADMIN" }
                });
        }
    }
}
