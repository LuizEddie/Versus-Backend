using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Versus.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTournamentColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_TournamentTypes_TypeId",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_TypeId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Tournaments");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "MatchDuration",
                table: "TournamentSpecs",
                type: "time(6)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "TournamentSpecs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TournamentSpecs_TypeId",
                table: "TournamentSpecs",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentSpecs_TournamentTypes_TypeId",
                table: "TournamentSpecs",
                column: "TypeId",
                principalTable: "TournamentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentSpecs_TournamentTypes_TypeId",
                table: "TournamentSpecs");

            migrationBuilder.DropIndex(
                name: "IX_TournamentSpecs_TypeId",
                table: "TournamentSpecs");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "TournamentSpecs");

            migrationBuilder.AlterColumn<string>(
                name: "MatchDuration",
                table: "TournamentSpecs",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time(6)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_TypeId",
                table: "Tournaments",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_TournamentTypes_TypeId",
                table: "Tournaments",
                column: "TypeId",
                principalTable: "TournamentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
