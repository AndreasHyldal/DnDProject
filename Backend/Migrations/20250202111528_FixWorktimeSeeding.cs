using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class FixWorktimeSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "DateOfBirth", "Email", "FirstName", "HireDate", "LastName", "PasswordHash", "Role" },
                values: new object[] { 1, new DateTime(1990, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "john.doe@example.com", "John", new DateTime(2025, 2, 2, 11, 15, 27, 314, DateTimeKind.Utc).AddTicks(707), "Doe", "VN5/YG8lI8uo76wXP6tC+39Z1Wzv+XTI/bc0LPLP40U=", "Employee" });

            migrationBuilder.InsertData(
                table: "Worktimes",
                columns: new[] { "Id", "EmployeeId", "End", "Start", "Task" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 2, 1, 17, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 2, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), "Worked on frontend UI" },
                    { 2, 1, new DateTime(2024, 2, 2, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 2, 2, 10, 0, 0, 0, DateTimeKind.Unspecified), "Bug fixes and testing" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Worktimes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Worktimes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
