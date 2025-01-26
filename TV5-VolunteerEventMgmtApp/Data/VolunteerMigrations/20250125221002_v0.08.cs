using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TV5_VolunteerEventMgmtApp.Data.VolunteerMigrations
{
    /// <inheritdoc />
    public partial class v008 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendeesSheets_Locations_LocationId",
                table: "AttendeesSheets");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendeesSheets_Locations_LocationId",
                table: "AttendeesSheets",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendeesSheets_Locations_LocationId",
                table: "AttendeesSheets");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendeesSheets_Locations_LocationId",
                table: "AttendeesSheets",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
