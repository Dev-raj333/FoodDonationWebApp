using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDonationWebApp.Migrations
{
    /// <inheritdoc />
    public partial class addedLattitude : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DropRequests_AllocatedFoods_AllocatedFoodID",
                table: "DropRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_DropRequests_AspNetUsers_VolunteerID",
                table: "DropRequests");

            migrationBuilder.AlterColumn<string>(
                name: "VolunteerID",
                table: "DropRequests",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PickupTime",
                table: "DropRequests",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "DropRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<int>(
                name: "AllocatedFoodID",
                table: "DropRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StreetName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DropRequests_AllocatedFoods_AllocatedFoodID",
                table: "DropRequests",
                column: "AllocatedFoodID",
                principalTable: "AllocatedFoods",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DropRequests_AspNetUsers_VolunteerID",
                table: "DropRequests",
                column: "VolunteerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DropRequests_AllocatedFoods_AllocatedFoodID",
                table: "DropRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_DropRequests_AspNetUsers_VolunteerID",
                table: "DropRequests");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StreetName",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "VolunteerID",
                table: "DropRequests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PickupTime",
                table: "DropRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "DropRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AllocatedFoodID",
                table: "DropRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DropRequests_AllocatedFoods_AllocatedFoodID",
                table: "DropRequests",
                column: "AllocatedFoodID",
                principalTable: "AllocatedFoods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DropRequests_AspNetUsers_VolunteerID",
                table: "DropRequests",
                column: "VolunteerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
