using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WyrdCodexAPI.Migrations
{
    /// <inheritdoc />
    public partial class indexingcards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardName",
                table: "Cards",
                column: "CardName");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_Faction",
                table: "Cards",
                column: "Faction");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_Type",
                table: "Cards",
                column: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cards_CardName",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_Faction",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_Type",
                table: "Cards");
        }
    }
}
