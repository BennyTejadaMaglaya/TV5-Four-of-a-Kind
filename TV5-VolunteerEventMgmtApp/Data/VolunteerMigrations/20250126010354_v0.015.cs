using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TV5_VolunteerEventMgmtApp.Data.VolunteerMigrations
{
    /// <inheritdoc />
    public partial class v0015 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DirectorLocations_Locations_LocationID",
                table: "DirectorLocations");

            migrationBuilder.AddForeignKey(
                name: "FK_DirectorLocations_Locations_LocationID",
                table: "DirectorLocations",
                column: "LocationID",
                principalTable: "Locations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DirectorLocations_Locations_LocationID",
                table: "DirectorLocations");

            migrationBuilder.AddForeignKey(
                name: "FK_DirectorLocations_Locations_LocationID",
                table: "DirectorLocations",
                column: "LocationID",
                principalTable: "Locations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
