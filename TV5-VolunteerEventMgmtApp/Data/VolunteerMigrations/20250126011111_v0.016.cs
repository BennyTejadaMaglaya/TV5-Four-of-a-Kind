using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TV5_VolunteerEventMgmtApp.Data.VolunteerMigrations
{
    /// <inheritdoc />
    public partial class v0016 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendeesSheets_Directors_DirectorId",
                table: "AttendeesSheets");

            migrationBuilder.AlterColumn<int>(
                name: "DirectorId",
                table: "AttendeesSheets",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendeesSheets_Directors_DirectorId",
                table: "AttendeesSheets",
                column: "DirectorId",
                principalTable: "Directors",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendeesSheets_Directors_DirectorId",
                table: "AttendeesSheets");

            migrationBuilder.AlterColumn<int>(
                name: "DirectorId",
                table: "AttendeesSheets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AttendeesSheets_Directors_DirectorId",
                table: "AttendeesSheets",
                column: "DirectorId",
                principalTable: "Directors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
