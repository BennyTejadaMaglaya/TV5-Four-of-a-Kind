using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TV5_VolunteerEventMgmtApp.Data.VolunteerMigrations
{
    /// <inheritdoc />
    public partial class v0012 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendees_Singers_SingerId",
                table: "Attendees");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendees_Singers_SingerId",
                table: "Attendees",
                column: "SingerId",
                principalTable: "Singers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendees_Singers_SingerId",
                table: "Attendees");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendees_Singers_SingerId",
                table: "Attendees",
                column: "SingerId",
                principalTable: "Singers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
