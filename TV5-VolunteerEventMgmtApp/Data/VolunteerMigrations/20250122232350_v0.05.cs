using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TV5_VolunteerEventMgmtApp.Data.VolunteerMigrations
{
    /// <inheritdoc />
    public partial class v005 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalSingers",
                table: "AttendeesSheets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalSingers",
                table: "AttendeesSheets");
        }
    }
}
