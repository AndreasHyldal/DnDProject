using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class SeedTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                column: "HireDate",
                value: new DateTime(2025, 2, 2, 11, 15, 56, 181, DateTimeKind.Utc).AddTicks(9842));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                column: "HireDate",
                value: new DateTime(2025, 2, 2, 11, 15, 27, 314, DateTimeKind.Utc).AddTicks(707));
        }
    }
}
