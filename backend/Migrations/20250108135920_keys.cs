using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WyrdCodexAPI.Migrations
{
    /// <inheritdoc />
    public partial class keys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserDecks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserDecks",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
