using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDonationWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsCancelledStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pickupDrop",
                table: "RecipientRequests");

            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "RecipientRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "RecipientRequests");

            migrationBuilder.AddColumn<int>(
                name: "pickupDrop",
                table: "RecipientRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
