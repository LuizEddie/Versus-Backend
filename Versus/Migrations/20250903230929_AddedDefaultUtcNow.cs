using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Versus.Migrations
{
    /// <inheritdoc />
    public partial class AddedDefaultUtcNow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Updated",
                table: "ResetPassword",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "ResetPassword",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<int>(
                name: "Used",
                table: "ResetPassword",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "ResetPassword",
                newName: "Updated");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ResetPassword",
                newName: "Created");

            migrationBuilder.AlterColumn<bool>(
                name: "Used",
                table: "ResetPassword",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
