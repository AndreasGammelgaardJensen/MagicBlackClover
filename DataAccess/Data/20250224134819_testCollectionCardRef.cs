using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Data
{
    /// <inheritdoc />
    public partial class testCollectionCardRef : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CollectionDatabaseModelId",
                table: "CollectionCards",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CollectionCards_CollectionDatabaseModelId",
                table: "CollectionCards",
                column: "CollectionDatabaseModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_CollectionCards_Collections_CollectionDatabaseModelId",
                table: "CollectionCards",
                column: "CollectionDatabaseModelId",
                principalTable: "Collections",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CollectionCards_Collections_CollectionDatabaseModelId",
                table: "CollectionCards");

            migrationBuilder.DropIndex(
                name: "IX_CollectionCards_CollectionDatabaseModelId",
                table: "CollectionCards");

            migrationBuilder.DropColumn(
                name: "CollectionDatabaseModelId",
                table: "CollectionCards");
        }
    }
}
