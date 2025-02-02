using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTestDataFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "DateOfBirth", "Email", "FirstName", "HireDate", "LastName", "PasswordHash", "Role" },
                values: new object[] { 2, new DateTime(1985, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "jane.doe@example.com", "Jane", new DateTime(2023, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Doe", "VN5/YG8lI8uo76wXP6tC+39Z1Wzv+XTI/bc0LPLP40U=", "Admin" });

            migrationBuilder.UpdateData(
                table: "Worktimes",
                keyColumn: "Id",
                keyValue: 8,
                column: "EmployeeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Worktimes",
                keyColumn: "Id",
                keyValue: 9,
                column: "EmployeeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Worktimes",
                keyColumn: "Id",
                keyValue: 10,
                column: "EmployeeId",
                value: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Worktimes",
                keyColumn: "Id",
                keyValue: 8,
                column: "EmployeeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Worktimes",
                keyColumn: "Id",
                keyValue: 9,
                column: "EmployeeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Worktimes",
                keyColumn: "Id",
                keyValue: 10,
                column: "EmployeeId",
                value: 1);
        }
    }
}
