using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDonationWebApp.Migrations
{
    /// <inheritdoc />
    public partial class DataTypeChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RequestedFoodType",
                table: "RecipientRequests",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RequestedFoodType",
                table: "RecipientRequests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
