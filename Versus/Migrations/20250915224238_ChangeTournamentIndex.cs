using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Versus.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTournamentIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentSpecs_Tournaments_TournamentId",
                table: "TournamentSpecs");

            migrationBuilder.DropIndex(
                name: "IX_TournamentSpecs_TournamentId",
                table: "TournamentSpecs");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentSpecs_TournamentId",
                table: "TournamentSpecs",
                column: "TournamentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentSpecs_Tournaments_TournamentId",
                table: "TournamentSpecs",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
              name: "FK_TournamentSpecs_Tournaments_TournamentId",
              table: "TournamentSpecs");

            migrationBuilder.DropIndex(
                name: "IX_TournamentSpecs_TournamentId",
                table: "TournamentSpecs");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentSpecs_TournamentId",
                table: "TournamentSpecs",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentSpecs_Tournaments_TournamentId",
                table: "TournamentSpecs",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
