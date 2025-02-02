using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Worktimes",
                columns: new[] { "Id", "EmployeeId", "End", "Start", "Task" },
                values: new object[,]
                {
                    { 3, 1, new DateTime(2024, 2, 3, 20, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 2, 3, 10, 0, 0, 0, DateTimeKind.Unspecified), "Bug fixes and testing" },
                    { 4, 1, new DateTime(2024, 2, 4, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 2, 4, 10, 0, 0, 0, DateTimeKind.Unspecified), "Bug fixes and testing" },
                    { 5, 1, new DateTime(2024, 2, 5, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 2, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), "Bug fixes and testing" },
                    { 6, 1, new DateTime(2024, 2, 6, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 2, 6, 10, 0, 0, 0, DateTimeKind.Unspecified), "Bug fixes and testing" },
                    { 7, 1, new DateTime(2024, 2, 7, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 2, 7, 10, 0, 0, 0, DateTimeKind.Unspecified), "Bug fixes and testing" },
                    { 8, 1, new DateTime(2024, 2, 1, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 2, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), "Bug fixes and testing" },
                    { 9, 1, new DateTime(2024, 2, 2, 22, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 2, 2, 10, 0, 0, 0, DateTimeKind.Unspecified), "Bug fixes and testing" },
                    { 10, 1, new DateTime(2024, 2, 3, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 2, 3, 10, 0, 0, 0, DateTimeKind.Unspecified), "Bug fixes and testing" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Worktimes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Worktimes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Worktimes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Worktimes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Worktimes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Worktimes",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Worktimes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Worktimes",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
