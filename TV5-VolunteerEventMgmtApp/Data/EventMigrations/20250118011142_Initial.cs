using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TV5_VolunteerEventMgmtApp.Data.EventMigrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    City = table.Column<string>(type: "TEXT", maxLength: 125, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Singers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    DOB = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Phone = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Singers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Directors",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    LocationID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Directors", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Directors_Locations_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Locations",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Venue",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VenueName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    ContactName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ContactPhone = table.Column<string>(type: "TEXT", nullable: true),
                    ContactEmail = table.Column<string>(type: "TEXT", nullable: true),
                    LocationId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venue", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Venue_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DirectorLocations",
                columns: table => new
                {
                    DirectorID = table.Column<int>(type: "INTEGER", nullable: false),
                    LocationID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectorLocations", x => new { x.LocationID, x.DirectorID });
                    table.ForeignKey(
                        name: "FK_DirectorLocations_Directors_DirectorID",
                        column: x => x.DirectorID,
                        principalTable: "Directors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DirectorLocations_Locations_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Locations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttendeesSheets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DirectorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LocationId = table.Column<int>(type: "INTEGER", nullable: false),
                    VenueId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendeesSheets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendeesSheets_Directors_DirectorId",
                        column: x => x.DirectorId,
                        principalTable: "Directors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendeesSheets_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendeesSheets_Venue_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venue",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attendees",
                columns: table => new
                {
                    SingerId = table.Column<int>(type: "INTEGER", nullable: false),
                    AttendenceSheetId = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendees", x => new { x.SingerId, x.AttendenceSheetId });
                    table.ForeignKey(
                        name: "FK_Attendees_AttendeesSheets_AttendenceSheetId",
                        column: x => x.AttendenceSheetId,
                        principalTable: "AttendeesSheets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Attendees_Singers_SingerId",
                        column: x => x.SingerId,
                        principalTable: "Singers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendees_AttendenceSheetId",
                table: "Attendees",
                column: "AttendenceSheetId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendeesSheets_DirectorId",
                table: "AttendeesSheets",
                column: "DirectorId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendeesSheets_LocationId",
                table: "AttendeesSheets",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendeesSheets_VenueId",
                table: "AttendeesSheets",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectorLocations_DirectorID",
                table: "DirectorLocations",
                column: "DirectorID");

            migrationBuilder.CreateIndex(
                name: "IX_Directors_LocationID",
                table: "Directors",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Venue_LocationId",
                table: "Venue",
                column: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendees");

            migrationBuilder.DropTable(
                name: "DirectorLocations");

            migrationBuilder.DropTable(
                name: "AttendeesSheets");

            migrationBuilder.DropTable(
                name: "Singers");

            migrationBuilder.DropTable(
                name: "Directors");

            migrationBuilder.DropTable(
                name: "Venue");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
