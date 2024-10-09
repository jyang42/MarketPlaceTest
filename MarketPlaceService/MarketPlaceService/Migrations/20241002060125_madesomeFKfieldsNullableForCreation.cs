using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketPlaceService.Migrations
{
    /// <inheritdoc />
    public partial class madesomeFKfieldsNullableForCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "expiration_date",
                table: "Jobs",
                newName: "ExpirationDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpirationDate",
                table: "Jobs",
                newName: "expiration_date");
        }
    }
}
