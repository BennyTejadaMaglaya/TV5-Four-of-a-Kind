using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TV5_VolunteerEventMgmtApp.Data.VolunteerMigrations
{
    /// <inheritdoc />
    public partial class V21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerEvent_Locations_LocationId",
                table: "VolunteerEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerEvent_Venues_VenueId",
                table: "VolunteerEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerSignups_VolunteerEvent_VolunteerEventId",
                table: "VolunteerSignups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VolunteerEvent",
                table: "VolunteerEvent");

            migrationBuilder.RenameTable(
                name: "VolunteerEvent",
                newName: "VolunteerEvents");

            migrationBuilder.RenameIndex(
                name: "IX_VolunteerEvent_VenueId",
                table: "VolunteerEvents",
                newName: "IX_VolunteerEvents_VenueId");

            migrationBuilder.RenameIndex(
                name: "IX_VolunteerEvent_LocationId",
                table: "VolunteerEvents",
                newName: "IX_VolunteerEvents_LocationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VolunteerEvents",
                table: "VolunteerEvents",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerEvents_Locations_LocationId",
                table: "VolunteerEvents",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerEvents_Venues_VenueId",
                table: "VolunteerEvents",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerSignups_VolunteerEvents_VolunteerEventId",
                table: "VolunteerSignups",
                column: "VolunteerEventId",
                principalTable: "VolunteerEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerEvents_Locations_LocationId",
                table: "VolunteerEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerEvents_Venues_VenueId",
                table: "VolunteerEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerSignups_VolunteerEvents_VolunteerEventId",
                table: "VolunteerSignups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VolunteerEvents",
                table: "VolunteerEvents");

            migrationBuilder.RenameTable(
                name: "VolunteerEvents",
                newName: "VolunteerEvent");

            migrationBuilder.RenameIndex(
                name: "IX_VolunteerEvents_VenueId",
                table: "VolunteerEvent",
                newName: "IX_VolunteerEvent_VenueId");

            migrationBuilder.RenameIndex(
                name: "IX_VolunteerEvents_LocationId",
                table: "VolunteerEvent",
                newName: "IX_VolunteerEvent_LocationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VolunteerEvent",
                table: "VolunteerEvent",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerEvent_Locations_LocationId",
                table: "VolunteerEvent",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerEvent_Venues_VenueId",
                table: "VolunteerEvent",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerSignups_VolunteerEvent_VolunteerEventId",
                table: "VolunteerSignups",
                column: "VolunteerEventId",
                principalTable: "VolunteerEvent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
