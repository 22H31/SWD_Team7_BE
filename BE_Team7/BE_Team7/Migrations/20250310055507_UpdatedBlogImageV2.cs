using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BE_Team7.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedBlogImageV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7f33484b-0f9e-4330-8306-0fb9cdfc0330");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "94c25269-358a-4d9d-9b4c-3df2940541af");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c9d42f25-1eb4-4c1f-a3f2-ae2bc156d46b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dc7a221a-672b-4c51-ae8e-0d66141da6a2");

            migrationBuilder.CreateTable(
                name: "BlogAvartarImage",
                columns: table => new
                {
                    BlogAvartarImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlogAvartarImageCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BlogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogAvartarImage", x => x.BlogAvartarImageId);
                    table.ForeignKey(
                        name: "FK_BlogAvartarImage_Blog_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blog",
                        principalColumn: "BlogId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogImage",
                columns: table => new
                {
                    BlogImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlogImageCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BlogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogImage", x => x.BlogImageId);
                    table.ForeignKey(
                        name: "FK_BlogImage_Blog_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blog",
                        principalColumn: "BlogId",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_BlogAvartarImage_BlogId",
                table: "BlogAvartarImage",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogImage_BlogId",
                table: "BlogImage",
                column: "BlogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogAvartarImage");

            migrationBuilder.DropTable(
                name: "BlogImage");

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
                    { "7f33484b-0f9e-4330-8306-0fb9cdfc0330", null, "User", "USER" },
                    { "94c25269-358a-4d9d-9b4c-3df2940541af", null, "StaffSale", "STAFFSALE" },
                    { "c9d42f25-1eb4-4c1f-a3f2-ae2bc156d46b", null, "Staff", "STAFF" },
                    { "dc7a221a-672b-4c51-ae8e-0d66141da6a2", null, "Admin", "ADMIN" }
                });
        }
    }
}
