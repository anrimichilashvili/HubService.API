using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HubService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedIsWinPorpertyToBets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsWin",
                table: "Bets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWin",
                table: "Bets");
        }
    }
}
