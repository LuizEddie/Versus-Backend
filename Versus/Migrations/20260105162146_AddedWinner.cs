using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Versus.Migrations
{
    /// <inheritdoc />
    public partial class AddedWinner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WinnerId",
                table: "Tournaments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_WinnerId",
                table: "Tournaments",
                column: "WinnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Players_WinnerId",
                table: "Tournaments",
                column: "WinnerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Players_WinnerId",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_WinnerId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "WinnerId",
                table: "Tournaments");
        }
    }
}
