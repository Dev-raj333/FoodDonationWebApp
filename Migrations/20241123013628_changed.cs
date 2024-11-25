using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDonationWebApp.Migrations
{
    /// <inheritdoc />
    public partial class changed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DropRequests_Donations_DonationID",
                table: "DropRequests");

            migrationBuilder.RenameColumn(
                name: "DonationID",
                table: "DropRequests",
                newName: "AllocatedFoodID");

            migrationBuilder.RenameIndex(
                name: "IX_DropRequests_DonationID",
                table: "DropRequests",
                newName: "IX_DropRequests_AllocatedFoodID");

            migrationBuilder.AddForeignKey(
                name: "FK_DropRequests_AllocatedFoods_AllocatedFoodID",
                table: "DropRequests",
                column: "AllocatedFoodID",
                principalTable: "AllocatedFoods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict); // Use Restrict to prevent cascading
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DropRequests_AllocatedFoods_AllocatedFoodID",
                table: "DropRequests");

            migrationBuilder.RenameColumn(
                name: "AllocatedFoodID",
                table: "DropRequests",
                newName: "DonationID");

            migrationBuilder.RenameIndex(
                name: "IX_DropRequests_AllocatedFoodID",
                table: "DropRequests",
                newName: "IX_DropRequests_DonationID");

            migrationBuilder.AddForeignKey(
                name: "FK_DropRequests_Donations_DonationID",
                table: "DropRequests",
                column: "DonationID",
                principalTable: "Donations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
