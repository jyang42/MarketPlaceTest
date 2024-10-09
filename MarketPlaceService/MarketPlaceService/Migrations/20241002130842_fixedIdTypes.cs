using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketPlaceService.Migrations
{
    /// <inheritdoc />
    public partial class fixedIdTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_AspNetUsers_PosterId",
                table: "Jobs");

            migrationBuilder.AlterColumn<string>(
                name: "PosterId",
                table: "Jobs",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BidUserId",
                table: "Bids",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_AspNetUsers_PosterId",
                table: "Jobs",
                column: "PosterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_AspNetUsers_PosterId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "BidUserId",
                table: "Bids");

            migrationBuilder.AlterColumn<string>(
                name: "PosterId",
                table: "Jobs",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_AspNetUsers_PosterId",
                table: "Jobs",
                column: "PosterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
