using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.Database.Migrations
{
    /// <inheritdoc />
    public partial class StockOnHoldSessionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "StockOnHold",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "StockOnHold");
        }
    }
}
