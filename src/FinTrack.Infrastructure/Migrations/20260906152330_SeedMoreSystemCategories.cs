using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FinTrack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedMoreSystemCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "IsSystemCategory", "Name", "ParentCategoryId", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000002"), new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Transportation and travel expenses", true, true, "Transportation", null, null, null },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shopping and purchases", true, true, "Shopping", null, null, null },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Utility and household bills", true, true, "Bills", null, null, null },
                    { new Guid("10000000-0000-0000-0000-000000000005"), new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Entertainment and leisure expenses", true, true, "Entertainment", null, null, null },
                    { new Guid("10000000-0000-0000-0000-000000000006"), new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Healthcare and medical expenses", true, true, "Health", null, null, null },
                    { new Guid("10000000-0000-0000-0000-000000000007"), new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Education and learning expenses", true, true, "Education", null, null, null },
                    { new Guid("10000000-0000-0000-0000-000000000008"), new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Recurring subscription expenses", true, true, "Subscriptions", null, null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
