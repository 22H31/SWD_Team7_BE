using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BE_Team7.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedBlogImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Blog",
                newName: "BlogCreatedAt");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Blog",
                newName: "Content2");

            migrationBuilder.AddColumn<string>(
                name: "Content1",
                table: "Blog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Content1",
                table: "Blog");

            migrationBuilder.RenameColumn(
                name: "Content2",
                table: "Blog",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "BlogCreatedAt",
                table: "Blog",
                newName: "CreatedAt");

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
    }
}
