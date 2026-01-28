using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Versus.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMatchDuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatchDuration",
                table: "TournamentSpecs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "MatchDuration",
                table: "TournamentSpecs",
                type: "time(6)",
                nullable: true);
        }
    }
}
