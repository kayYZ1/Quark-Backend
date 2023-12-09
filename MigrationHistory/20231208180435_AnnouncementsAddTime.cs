using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quark_Backend.MigrationHistory
{
    /// <inheritdoc />
    public partial class AnnouncementsAddTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Time",
                table: "announcements",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "announcements");
        }
    }
}
