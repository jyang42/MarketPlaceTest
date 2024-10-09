using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketPlaceService.Migrations
{
    /// <inheritdoc />
    public partial class morefk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_AspNetUsers_BidderId",
                table: "Bids");

            migrationBuilder.DropIndex(
                name: "IX_Bids_BidderId",
                table: "Bids");

            migrationBuilder.AlterColumn<int>(
                name: "BidderId",
                table: "Bids",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BidderId1",
                table: "Bids",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bids_BidderId1",
                table: "Bids",
                column: "BidderId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_AspNetUsers_BidderId1",
                table: "Bids",
                column: "BidderId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_AspNetUsers_BidderId1",
                table: "Bids");

            migrationBuilder.DropIndex(
                name: "IX_Bids_BidderId1",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "BidderId1",
                table: "Bids");

            migrationBuilder.AlterColumn<string>(
                name: "BidderId",
                table: "Bids",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_BidderId",
                table: "Bids",
                column: "BidderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_AspNetUsers_BidderId",
                table: "Bids",
                column: "BidderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
