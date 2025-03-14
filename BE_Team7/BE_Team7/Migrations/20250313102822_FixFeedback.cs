using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BE_Team7.Migrations
{
    /// <inheritdoc />
    public partial class FixFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_AspNetUsers_UserId",
                table: "Feedback");

            migrationBuilder.DropIndex(
                name: "IX_Feedback_UserId",
                table: "Feedback");

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

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Feedback");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Feedback",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "16a5f2dd-f4db-411a-a8e4-7aa9ae0cde80", null, "User", "USER" },
                    { "317a7aa9-ef8b-4a95-a717-cf616ae5bf20", null, "Staff", "STAFF" },
                    { "7526336d-65b8-433b-8a76-a298344b4036", null, "Admin", "ADMIN" },
                    { "97bc814e-b4ec-48ff-953f-26fcaf00e2e1", null, "StaffSale", "STAFFSALE" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_Id",
                table: "Feedback",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_AspNetUsers_Id",
                table: "Feedback",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_AspNetUsers_Id",
                table: "Feedback");

            migrationBuilder.DropIndex(
                name: "IX_Feedback_Id",
                table: "Feedback");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "16a5f2dd-f4db-411a-a8e4-7aa9ae0cde80");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "317a7aa9-ef8b-4a95-a717-cf616ae5bf20");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7526336d-65b8-433b-8a76-a298344b4036");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "97bc814e-b4ec-48ff-953f-26fcaf00e2e1");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Feedback",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Feedback",
                type: "nvarchar(450)",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_UserId",
                table: "Feedback",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_AspNetUsers_UserId",
                table: "Feedback",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
