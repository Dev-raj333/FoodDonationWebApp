using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDonationWebApp.Migrations
{
    /// <inheritdoc />
    public partial class FoodStatusAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FoodStatus",
                table: "inventories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoodStatus",
                table: "inventories");
        }
    }
}
