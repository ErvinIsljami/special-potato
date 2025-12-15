using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VuDrive.Migrations
{
    /// <inheritdoc />
    public partial class AddProductSetCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "AppProductSets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "AppDisplays",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "AppProductSets");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "AppDisplays");
        }
    }
}
