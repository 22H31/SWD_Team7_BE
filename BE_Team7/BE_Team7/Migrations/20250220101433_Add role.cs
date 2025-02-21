using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BE_Team7.Migrations
{
    /// <inheritdoc />
    public partial class Addrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoutineId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "SkinType",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "001e9f32-21dd-49e3-b0a4-87b7f99413e0", null, "Admin", "ADMIN" },
                    { "5d852744-77ab-4764-bc6c-28208cc1e291", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "001e9f32-21dd-49e3-b0a4-87b7f99413e0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5d852744-77ab-4764-bc6c-28208cc1e291");

            migrationBuilder.AlterColumn<string>(
                name: "SkinType",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoutineId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }
    }
}
