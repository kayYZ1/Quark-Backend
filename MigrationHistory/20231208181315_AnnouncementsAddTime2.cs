using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quark_Backend.MigrationHistory
{
    /// <inheritdoc />
    public partial class AnnouncementsAddTime2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "announcements",
                newName: "time");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "time",
                table: "announcements",
                newName: "Time");
        }
    }
}
