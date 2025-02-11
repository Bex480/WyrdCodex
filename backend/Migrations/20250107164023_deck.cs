using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WyrdCodexAPI.Migrations
{
    /// <inheritdoc />
    public partial class deck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeckCards_Deck_DeckId",
                table: "DeckCards");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDecks_Deck_DeckId",
                table: "UserDecks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Deck",
                table: "Deck");

            migrationBuilder.RenameTable(
                name: "Deck",
                newName: "Decks");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Decks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Decks",
                table: "Decks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeckCards_Decks_DeckId",
                table: "DeckCards",
                column: "DeckId",
                principalTable: "Decks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDecks_Decks_DeckId",
                table: "UserDecks",
                column: "DeckId",
                principalTable: "Decks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeckCards_Decks_DeckId",
                table: "DeckCards");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDecks_Decks_DeckId",
                table: "UserDecks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Decks",
                table: "Decks");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Decks");

            migrationBuilder.RenameTable(
                name: "Decks",
                newName: "Deck");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Deck",
                table: "Deck",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeckCards_Deck_DeckId",
                table: "DeckCards",
                column: "DeckId",
                principalTable: "Deck",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDecks_Deck_DeckId",
                table: "UserDecks",
                column: "DeckId",
                principalTable: "Deck",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
