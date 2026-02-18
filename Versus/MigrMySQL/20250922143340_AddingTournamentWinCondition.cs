using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Versus.Migrations
{
    /// <inheritdoc />
    public partial class AddingTournamentWinCondition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TieBreaker",
                table: "TournamentSpecs");

            migrationBuilder.DropColumn(
                name: "WinCondition",
                table: "TournamentSpecs");

            migrationBuilder.AddColumn<int>(
                name: "QtyGroups",
                table: "TournamentSpecs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TieBreakerId",
                table: "TournamentSpecs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WinConditionId",
                table: "TournamentSpecs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Tournaments",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Tournaments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Group",
                table: "Matches",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TournamentTieBreaker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentTieBreaker", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TournamentWinConditon",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentWinConditon", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentSpecs_TieBreakerId",
                table: "TournamentSpecs",
                column: "TieBreakerId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentSpecs_WinConditionId",
                table: "TournamentSpecs",
                column: "WinConditionId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentSpecs_TournamentTieBreaker_TieBreakerId",
                table: "TournamentSpecs");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentSpecs_TournamentWinConditon_WinConditionId",
                table: "TournamentSpecs");

            migrationBuilder.DropTable(
                name: "TournamentTieBreaker");

            migrationBuilder.DropTable(
                name: "TournamentWinConditon");

            migrationBuilder.DropIndex(
                name: "IX_TournamentSpecs_TieBreakerId",
                table: "TournamentSpecs");

            migrationBuilder.DropIndex(
                name: "IX_TournamentSpecs_WinConditionId",
                table: "TournamentSpecs");

            migrationBuilder.DropColumn(
                name: "QtyGroups",
                table: "TournamentSpecs");

            migrationBuilder.DropColumn(
                name: "TieBreakerId",
                table: "TournamentSpecs");

            migrationBuilder.DropColumn(
                name: "WinConditionId",
                table: "TournamentSpecs");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "Matches");

            migrationBuilder.AddColumn<string>(
                name: "TieBreaker",
                table: "TournamentSpecs",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "WinCondition",
                table: "TournamentSpecs",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
