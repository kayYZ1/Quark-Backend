using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Quark_Backend.MigrationHistory
{
    /// <inheritdoc />
    public partial class SeedExampleData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "departments",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "IT" },
                    { 2, "HR" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "ConnectionId", "email", "first_name", "job_id", "last_name", "password", "permission_level", "picture_url", "self_description", "username" },
                values: new object[,]
                {
                    { 1, null, "adam.kowalski@gmail.com", null, null, null, "1234", null, null, null, null },
                    { 2, null, "jan.nowak@gmail.com", null, null, null, "1234", null, null, null, null },
                    { 3, null, "weronika.kowalczyk@gmail.com", null, null, null, "1234", null, null, null, null },
                    { 4, null, "adrianna.lewandowska@gmail.com", null, null, null, "1234", null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "job_positions",
                columns: new[] { "id", "department_id", "name" },
                values: new object[,]
                {
                    { 1, 1, "Junior Web Developer" },
                    { 2, 1, "Mid Web Developer" },
                    { 3, 1, "Senior Web Developer" },
                    { 4, 1, "Junior Software Developer" },
                    { 5, 1, "Mid Software Developer" },
                    { 6, 1, "Senior Software Developer" },
                    { 7, 2, "Recruiter" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "job_positions",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "job_positions",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "job_positions",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "job_positions",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "job_positions",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "job_positions",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "job_positions",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "departments",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "departments",
                keyColumn: "id",
                keyValue: 2);
        }
    }
}
