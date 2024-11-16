using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDonationWebApp.Migrations
{
    /// <inheritdoc />
    public partial class StatusAddedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CancelledStatus",
                table: "PickupRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelledStatus",
                table: "PickupRequests");
        }
    }
}
