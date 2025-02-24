using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Data
{
    /// <inheritdoc />
    public partial class AddCard5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Flavor",
                table: "ForeignNames");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ForeignNames");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "ForeignNames");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ForeignNames");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Flavor",
                table: "ForeignNames",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ForeignNames",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "ForeignNames",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ForeignNames",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
