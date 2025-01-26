using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TV5_VolunteerEventMgmtApp.Data.VolunteerMigrations
{
    /// <inheritdoc />
    public partial class v007 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DirectorLocations_Directors_DirectorID",
                table: "DirectorLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_SingerLocations_Singers_SingerId",
                table: "SingerLocations");

            migrationBuilder.CreateIndex(
                name: "IX_Singers_FirstName_LastName_DOB",
                table: "Singers",
                columns: new[] { "FirstName", "LastName", "DOB" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_City",
                table: "Locations",
                column: "City",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Directors_Email",
                table: "Directors",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DirectorLocations_Directors_DirectorID",
                table: "DirectorLocations",
                column: "DirectorID",
                principalTable: "Directors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SingerLocations_Singers_SingerId",
                table: "SingerLocations",
                column: "SingerId",
                principalTable: "Singers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DirectorLocations_Directors_DirectorID",
                table: "DirectorLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_SingerLocations_Singers_SingerId",
                table: "SingerLocations");

            migrationBuilder.DropIndex(
                name: "IX_Singers_FirstName_LastName_DOB",
                table: "Singers");

            migrationBuilder.DropIndex(
                name: "IX_Locations_City",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Directors_Email",
                table: "Directors");

            migrationBuilder.AddForeignKey(
                name: "FK_DirectorLocations_Directors_DirectorID",
                table: "DirectorLocations",
                column: "DirectorID",
                principalTable: "Directors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SingerLocations_Singers_SingerId",
                table: "SingerLocations",
                column: "SingerId",
                principalTable: "Singers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
