using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HubService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removedUSerIDMadeItGeneral : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Bets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Bets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
