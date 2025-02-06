using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TV5_VolunteerEventMgmtApp.Data.VolunteerMigrations
{
    /// <inheritdoc />
    public partial class V20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Venues",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumPastEvents",
                table: "Venues",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "VolunteerEvent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 125, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    LocationId = table.Column<int>(type: "INTEGER", nullable: false),
                    VenueId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VolunteerEvent_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VolunteerEvent_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Volunteers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 125, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 125, nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    EmailAddress = table.Column<string>(type: "TEXT", nullable: false),
                    JoinDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TimesLate = table.Column<int>(type: "INTEGER", nullable: false),
                    numShifts = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsConfirmed = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Volunteers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VolunteerSignups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TimeSlots = table.Column<int>(type: "INTEGER", nullable: false),
                    VolunteerEventId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerSignups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VolunteerSignups_VolunteerEvent_VolunteerEventId",
                        column: x => x.VolunteerEventId,
                        principalTable: "VolunteerEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VolunteerLocations",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "INTEGER", nullable: false),
                    VolunteerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerLocations", x => new { x.VolunteerId, x.LocationId });
                    table.ForeignKey(
                        name: "FK_VolunteerLocations_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VolunteerLocations_Volunteers_VolunteerId",
                        column: x => x.VolunteerId,
                        principalTable: "Volunteers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VolunteerAttendees",
                columns: table => new
                {
                    VolunteerId = table.Column<int>(type: "INTEGER", nullable: false),
                    VolunteerSignupId = table.Column<int>(type: "INTEGER", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DepartureTime = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerAttendees", x => new { x.VolunteerSignupId, x.VolunteerId });
                    table.ForeignKey(
                        name: "FK_VolunteerAttendees_VolunteerSignups_VolunteerSignupId",
                        column: x => x.VolunteerSignupId,
                        principalTable: "VolunteerSignups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VolunteerAttendees_Volunteers_VolunteerId",
                        column: x => x.VolunteerId,
                        principalTable: "Volunteers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerAttendees_VolunteerId_VolunteerSignupId",
                table: "VolunteerAttendees",
                columns: new[] { "VolunteerId", "VolunteerSignupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerEvent_LocationId",
                table: "VolunteerEvent",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerEvent_VenueId",
                table: "VolunteerEvent",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerLocations_LocationId",
                table: "VolunteerLocations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Volunteers_EmailAddress",
                table: "Volunteers",
                column: "EmailAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerSignups_VolunteerEventId",
                table: "VolunteerSignups",
                column: "VolunteerEventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VolunteerAttendees");

            migrationBuilder.DropTable(
                name: "VolunteerLocations");

            migrationBuilder.DropTable(
                name: "VolunteerSignups");

            migrationBuilder.DropTable(
                name: "Volunteers");

            migrationBuilder.DropTable(
                name: "VolunteerEvent");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "NumPastEvents",
                table: "Venues");
        }
    }
}
