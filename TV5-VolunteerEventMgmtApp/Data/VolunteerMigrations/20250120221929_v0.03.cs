using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TV5_VolunteerEventMgmtApp.Data.VolunteerMigrations
{
    /// <inheritdoc />
    public partial class v003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendeesSheets_Venues_VenueId",
                table: "AttendeesSheets");

            migrationBuilder.DropIndex(
                name: "IX_AttendeesSheets_VenueId",
                table: "AttendeesSheets");

            migrationBuilder.DropColumn(
                name: "VenueId",
                table: "AttendeesSheets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VenueId",
                table: "AttendeesSheets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AttendeesSheets_VenueId",
                table: "AttendeesSheets",
                column: "VenueId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendeesSheets_Venues_VenueId",
                table: "AttendeesSheets",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
