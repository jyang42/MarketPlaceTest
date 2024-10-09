using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketPlaceService.Migrations
{
    /// <inheritdoc />
    public partial class RenamedColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_AspNetUsers_UserId",
                table: "Bids");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Bids",
                newName: "BidderId");

            migrationBuilder.RenameIndex(
                name: "IX_Bids_UserId",
                table: "Bids",
                newName: "IX_Bids_BidderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_AspNetUsers_BidderId",
                table: "Bids",
                column: "BidderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_AspNetUsers_BidderId",
                table: "Bids");

            migrationBuilder.RenameColumn(
                name: "BidderId",
                table: "Bids",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Bids_BidderId",
                table: "Bids",
                newName: "IX_Bids_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_AspNetUsers_UserId",
                table: "Bids",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
