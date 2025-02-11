using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WyrdCodexAPI.Migrations
{
    /// <inheritdoc />
    public partial class key : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCards",
                table: "UserCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeckCards",
                table: "DeckCards");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UserCards",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DeckCards",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCards",
                table: "UserCards",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeckCards",
                table: "DeckCards",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserCards_UserId",
                table: "UserCards",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DeckCards_DeckId",
                table: "DeckCards",
                column: "DeckId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCards",
                table: "UserCards");

            migrationBuilder.DropIndex(
                name: "IX_UserCards_UserId",
                table: "UserCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeckCards",
                table: "DeckCards");

            migrationBuilder.DropIndex(
                name: "IX_DeckCards_DeckId",
                table: "DeckCards");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UserCards",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DeckCards",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCards",
                table: "UserCards",
                columns: new[] { "UserId", "CardId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeckCards",
                table: "DeckCards",
                columns: new[] { "DeckId", "CardId" });
        }
    }
}
