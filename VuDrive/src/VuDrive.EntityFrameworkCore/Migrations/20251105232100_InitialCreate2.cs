using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VuDrive.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppCars_AppProductSets_ProductSetId",
                table: "AppCars");

            migrationBuilder.DropTable(
                name: "AppDisplays");

            migrationBuilder.DropIndex(
                name: "IX_AppCars_ProductSetId",
                table: "AppCars");

            migrationBuilder.DropColumn(
                name: "ProductSetId",
                table: "AppCars");

            migrationBuilder.DropColumn(
                name: "YearsBuiltFrom",
                table: "AppCars");

            migrationBuilder.DropColumn(
                name: "YearsBuiltTo",
                table: "AppCars");

            migrationBuilder.AddColumn<string>(
                name: "YearsBuilt",
                table: "AppCars",
                type: "TEXT",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AppProductSetCars",
                columns: table => new
                {
                    ProductSetId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CarId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppProductSetCars", x => new { x.ProductSetId, x.CarId });
                    table.ForeignKey(
                        name: "FK_AppProductSetCars_AppCars_CarId",
                        column: x => x.CarId,
                        principalTable: "AppCars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppProductSetCars_AppProductSets_ProductSetId",
                        column: x => x.ProductSetId,
                        principalTable: "AppProductSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppProductSetCars_CarId",
                table: "AppProductSetCars",
                column: "CarId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppProductSetCars");

            migrationBuilder.DropColumn(
                name: "YearsBuilt",
                table: "AppCars");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductSetId",
                table: "AppCars",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YearsBuiltFrom",
                table: "AppCars",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YearsBuiltTo",
                table: "AppCars",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AppDisplays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AndroidVersion = table.Column<string>(type: "TEXT", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Cpu = table.Column<string>(type: "TEXT", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatorId = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeleterId = table.Column<Guid>(type: "TEXT", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ExtraProperties = table.Column<string>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    LastModificationTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Memory = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Ram = table.Column<int>(type: "INTEGER", nullable: false),
                    SizeInInches = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppDisplays", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppCars_ProductSetId",
                table: "AppCars",
                column: "ProductSetId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppCars_AppProductSets_ProductSetId",
                table: "AppCars",
                column: "ProductSetId",
                principalTable: "AppProductSets",
                principalColumn: "Id");
        }
    }
}
