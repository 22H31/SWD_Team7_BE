using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BE_Team7.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedProductAvartarImageV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAvatarImage_Products_ProductId",
                table: "ProductAvatarImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductAvatarImage",
                table: "ProductAvatarImage");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0afcf45b-5c74-42f2-8dbc-f96775c335c1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4b375e88-ed81-4835-9053-0712a79fde57");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5bcd5ffa-b3ea-44d6-b136-1f9f758fa0b3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bdc8594d-b6b2-4a54-8410-7e77d5169252");

            migrationBuilder.RenameTable(
                name: "ProductAvatarImage",
                newName: "productAvatarImage");

            migrationBuilder.RenameIndex(
                name: "IX_ProductAvatarImage_ProductId",
                table: "productAvatarImage",
                newName: "IX_productAvatarImage_ProductId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ProductImageCreatedAt",
                table: "ProductImage",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ProductAvatarImageCreatedAt",
                table: "productAvatarImage",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_productAvatarImage",
                table: "productAvatarImage",
                column: "ProductAvartarrImageId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_productAvatarImage_Products_ProductId",
                table: "productAvatarImage",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productAvatarImage_Products_ProductId",
                table: "productAvatarImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_productAvatarImage",
                table: "productAvatarImage");

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

            migrationBuilder.DropColumn(
                name: "ProductImageCreatedAt",
                table: "ProductImage");

            migrationBuilder.DropColumn(
                name: "ProductAvatarImageCreatedAt",
                table: "productAvatarImage");

            migrationBuilder.RenameTable(
                name: "productAvatarImage",
                newName: "ProductAvatarImage");

            migrationBuilder.RenameIndex(
                name: "IX_productAvatarImage_ProductId",
                table: "ProductAvatarImage",
                newName: "IX_ProductAvatarImage_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductAvatarImage",
                table: "ProductAvatarImage",
                column: "ProductAvartarrImageId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0afcf45b-5c74-42f2-8dbc-f96775c335c1", null, "User", "USER" },
                    { "4b375e88-ed81-4835-9053-0712a79fde57", null, "Staff", "STAFF" },
                    { "5bcd5ffa-b3ea-44d6-b136-1f9f758fa0b3", null, "Admin", "ADMIN" },
                    { "bdc8594d-b6b2-4a54-8410-7e77d5169252", null, "StaffSale", "STAFFSALE" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAvatarImage_Products_ProductId",
                table: "ProductAvatarImage",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
