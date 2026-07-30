using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace todo_api.Migrations
{
    /// <inheritdoc />
    public partial class SeedTodo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "CompletedAt", "IsCompleted", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 7, 30, 11, 32, 55, 515, DateTimeKind.Utc).AddTicks(5207), false, "Sample Todo 1" },
                    { 2, new DateTime(2026, 7, 30, 11, 32, 55, 515, DateTimeKind.Utc).AddTicks(5452), true, "Sample Todo 2" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
