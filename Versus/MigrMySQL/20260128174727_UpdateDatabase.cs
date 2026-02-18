using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Versus.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentSpecs_TournamentTieBreaker_TieBreakerId",
                table: "TournamentSpecs");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentSpecs_TournamentWinConditon_WinConditionId",
                table: "TournamentSpecs");

            migrationBuilder.DropTable(
                name: "Classifications");

            migrationBuilder.DropIndex(
                name: "IX_TournamentSpecs_TieBreakerId",
                table: "TournamentSpecs");

            migrationBuilder.DropColumn(
                name: "PointsPerDraw",
                table: "TournamentSpecs");

            migrationBuilder.DropColumn(
                name: "PointsPerLose",
                table: "TournamentSpecs");

            migrationBuilder.DropColumn(
                name: "PointsPerWin",
                table: "TournamentSpecs");

            migrationBuilder.DropColumn(
                name: "QtyGroups",
                table: "TournamentSpecs");

            migrationBuilder.DropColumn(
                name: "QtyRounds",
                table: "TournamentSpecs");

            migrationBuilder.DropColumn(
                name: "TieBreakerId",
                table: "TournamentSpecs");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Phase",
                table: "Matches");

            migrationBuilder.AlterColumn<int>(
                name: "WinConditionId",
                table: "TournamentSpecs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentSpecs_TournamentWinConditon_WinConditionId",
                table: "TournamentSpecs",
                column: "WinConditionId",
                principalTable: "TournamentWinConditon",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentSpecs_TournamentWinConditon_WinConditionId",
                table: "TournamentSpecs");

            migrationBuilder.AlterColumn<int>(
                name: "WinConditionId",
                table: "TournamentSpecs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PointsPerDraw",
                table: "TournamentSpecs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PointsPerLose",
                table: "TournamentSpecs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PointsPerWin",
                table: "TournamentSpecs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QtyGroups",
                table: "TournamentSpecs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QtyRounds",
                table: "TournamentSpecs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TieBreakerId",
                table: "TournamentSpecs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Group",
                table: "Matches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phase",
                table: "Matches",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Classifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    TournamentId = table.Column<int>(type: "int", nullable: false),
                    Draws = table.Column<int>(type: "int", nullable: false),
                    Loses = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    Wins = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classifications_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Classifications_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentSpecs_TieBreakerId",
                table: "TournamentSpecs",
                column: "TieBreakerId");

            migrationBuilder.CreateIndex(
                name: "IX_Classifications_PlayerId",
                table: "Classifications",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Classifications_TournamentId",
                table: "Classifications",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentSpecs_TournamentTieBreaker_TieBreakerId",
                table: "TournamentSpecs",
                column: "TieBreakerId",
                principalTable: "TournamentTieBreaker",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentSpecs_TournamentWinConditon_WinConditionId",
                table: "TournamentSpecs",
                column: "WinConditionId",
                principalTable: "TournamentWinConditon",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
