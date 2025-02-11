using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WyrdCodexAPI.Migrations
{
    /// <inheritdoc />
    public partial class cart_changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Carts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Carts",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
