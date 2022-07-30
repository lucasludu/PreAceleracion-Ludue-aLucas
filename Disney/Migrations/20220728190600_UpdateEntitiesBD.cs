using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Disney.Migrations
{
    public partial class UpdateEntitiesBD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Movies",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Genres",
                newName: "Image");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Movies",
                newName: "ImagePath");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Genres",
                newName: "ImagePath");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
