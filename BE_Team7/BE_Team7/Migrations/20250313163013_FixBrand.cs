using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BE_Team7.Migrations
{
    /// <inheritdoc />
    public partial class FixBrand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "CategoryTitleIcon",
                table: "CategoryTitle");

            migrationBuilder.RenameColumn(
                name: "BrandImg",
                table: "Brand",
                newName: "brandDescription");

            migrationBuilder.CreateTable(
                name: "BrandAvartarImage",
                columns: table => new
                {
                    BrandAvartarImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrandAvartarImageCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BrandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandAvartarImage", x => x.BrandAvartarImageId);
                    table.ForeignKey(
                        name: "FK_BrandAvartarImage_Brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brand",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_BrandAvartarImage_BrandId",
                table: "BrandAvartarImage",
                column: "BrandId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrandAvartarImage");

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

            migrationBuilder.RenameColumn(
                name: "brandDescription",
                table: "Brand",
                newName: "BrandImg");

            migrationBuilder.AddColumn<string>(
                name: "CategoryTitleIcon",
                table: "CategoryTitle",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
        }
    }
}
