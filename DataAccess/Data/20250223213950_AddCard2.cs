using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Data
{
    /// <inheritdoc />
    public partial class AddCard2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ScanId",
                table: "ScanItems",
                newName: "CardId");

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManaCost = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cmc = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rarity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Set = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SetName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Artist = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Power = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Toughness = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Layout = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MultiverseId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ForeignNames",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Flavor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MultiverseId = table.Column<int>(type: "int", nullable: false),
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForeignNames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForeignNames_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Legalities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Format = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LegalityStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Legalities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Legalities_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Printings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Set = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Printings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Printings_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScanItems_CardId",
                table: "ScanItems",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_ForeignNames_CardId",
                table: "ForeignNames",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Legalities_CardId",
                table: "Legalities",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Printings_CardId",
                table: "Printings",
                column: "CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScanItems_Cards_CardId",
                table: "ScanItems",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScanItems_Cards_CardId",
                table: "ScanItems");

            migrationBuilder.DropTable(
                name: "ForeignNames");

            migrationBuilder.DropTable(
                name: "Legalities");

            migrationBuilder.DropTable(
                name: "Printings");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_ScanItems_CardId",
                table: "ScanItems");

            migrationBuilder.RenameColumn(
                name: "CardId",
                table: "ScanItems",
                newName: "ScanId");
        }
    }
}
