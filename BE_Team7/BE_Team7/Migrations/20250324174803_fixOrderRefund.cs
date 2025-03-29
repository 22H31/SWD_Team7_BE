using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BE_Team7.Migrations
{
    /// <inheritdoc />
    public partial class fixOrderRefund : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_orderRefunds_OrderId",
                table: "orderRefunds");

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

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "orderRefunds",
                newName: "OrderRefundStatus");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId1",
                table: "orderRefunds",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1b3dd6e3-20f8-402c-9bf1-f6cb8f5ac1a1", null, "StaffSale", "STAFFSALE" },
                    { "262ba915-8dbc-4332-85e5-b46ccaa38c00", null, "Staff", "STAFF" },
                    { "30bee3a6-fdd6-4d16-a933-2b24203c59f0", null, "Admin", "ADMIN" },
                    { "37d030a6-660d-44bb-ba27-5953649214bc", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_orderRefunds_OrderId",
                table: "orderRefunds",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_orderRefunds_OrderId1",
                table: "orderRefunds",
                column: "OrderId1",
                unique: true,
                filter: "[OrderId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_orderRefunds_Order_OrderId1",
                table: "orderRefunds",
                column: "OrderId1",
                principalTable: "Order",
                principalColumn: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderRefunds_Order_OrderId1",
                table: "orderRefunds");

            migrationBuilder.DropIndex(
                name: "IX_orderRefunds_OrderId",
                table: "orderRefunds");

            migrationBuilder.DropIndex(
                name: "IX_orderRefunds_OrderId1",
                table: "orderRefunds");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1b3dd6e3-20f8-402c-9bf1-f6cb8f5ac1a1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "262ba915-8dbc-4332-85e5-b46ccaa38c00");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "30bee3a6-fdd6-4d16-a933-2b24203c59f0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "37d030a6-660d-44bb-ba27-5953649214bc");

            migrationBuilder.DropColumn(
                name: "OrderId1",
                table: "orderRefunds");

            migrationBuilder.RenameColumn(
                name: "OrderRefundStatus",
                table: "orderRefunds",
                newName: "Status");

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

            migrationBuilder.CreateIndex(
                name: "IX_orderRefunds_OrderId",
                table: "orderRefunds",
                column: "OrderId",
                unique: true);
        }
    }
}
