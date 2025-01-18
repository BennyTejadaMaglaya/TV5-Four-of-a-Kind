using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TV5_VolunteerEventMgmtApp.Data.EventMigrations
{
    /// <inheritdoc />
    public partial class V1001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendeesSheets_Venue_VenueId",
                table: "AttendeesSheets");

            migrationBuilder.DropForeignKey(
                name: "FK_Directors_Locations_LocationID",
                table: "Directors");

            migrationBuilder.DropForeignKey(
                name: "FK_Venue_Locations_LocationId",
                table: "Venue");

            migrationBuilder.DropIndex(
                name: "IX_Directors_LocationID",
                table: "Directors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Venue",
                table: "Venue");

            migrationBuilder.DropColumn(
                name: "LocationID",
                table: "Directors");

            migrationBuilder.RenameTable(
                name: "Venue",
                newName: "Venues");

            migrationBuilder.RenameIndex(
                name: "IX_Venue_LocationId",
                table: "Venues",
                newName: "IX_Venues_LocationId");

            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "Singers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Venues",
                table: "Venues",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "SingerLocations",
                columns: table => new
                {
                    SingerId = table.Column<int>(type: "INTEGER", nullable: false),
                    LocationId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SingerLocations", x => new { x.SingerId, x.LocationId });
                    table.ForeignKey(
                        name: "FK_SingerLocations_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SingerLocations_Singers_SingerId",
                        column: x => x.SingerId,
                        principalTable: "Singers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SingerLocations_LocationId",
                table: "SingerLocations",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendeesSheets_Venues_VenueId",
                table: "AttendeesSheets",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Venues_Locations_LocationId",
                table: "Venues",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendeesSheets_Venues_VenueId",
                table: "AttendeesSheets");

            migrationBuilder.DropForeignKey(
                name: "FK_Venues_Locations_LocationId",
                table: "Venues");

            migrationBuilder.DropTable(
                name: "SingerLocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Venues",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "Singers");

            migrationBuilder.RenameTable(
                name: "Venues",
                newName: "Venue");

            migrationBuilder.RenameIndex(
                name: "IX_Venues_LocationId",
                table: "Venue",
                newName: "IX_Venue_LocationId");

            migrationBuilder.AddColumn<int>(
                name: "LocationID",
                table: "Directors",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Venue",
                table: "Venue",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Directors_LocationID",
                table: "Directors",
                column: "LocationID");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendeesSheets_Venue_VenueId",
                table: "AttendeesSheets",
                column: "VenueId",
                principalTable: "Venue",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Directors_Locations_LocationID",
                table: "Directors",
                column: "LocationID",
                principalTable: "Locations",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Venue_Locations_LocationId",
                table: "Venue",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
