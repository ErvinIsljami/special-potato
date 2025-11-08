using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VuDrive.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AppProductSetCars",
                table: "AppProductSetCars");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "AppProductSetCars",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppProductSetCars",
                table: "AppProductSetCars",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AppDisplays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    SizeInInches = table.Column<decimal>(type: "TEXT", nullable: false),
                    AndroidVersion = table.Column<string>(type: "TEXT", nullable: false),
                    Ram = table.Column<int>(type: "INTEGER", nullable: false),
                    Memory = table.Column<int>(type: "INTEGER", nullable: false),
                    Cpu = table.Column<string>(type: "TEXT", nullable: false),
                    ExtraProperties = table.Column<string>(type: "TEXT", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatorId = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppDisplays", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppProductSetCars_ProductSetId_CarId",
                table: "AppProductSetCars",
                columns: new[] { "ProductSetId", "CarId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppDisplays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppProductSetCars",
                table: "AppProductSetCars");

            migrationBuilder.DropIndex(
                name: "IX_AppProductSetCars_ProductSetId_CarId",
                table: "AppProductSetCars");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AppProductSetCars");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppProductSetCars",
                table: "AppProductSetCars",
                columns: new[] { "ProductSetId", "CarId" });
        }
    }
}
