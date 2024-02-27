using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_AnimeToNotion.Migrations
{
    /// <inheritdoc />
    public partial class EDIT_ANIMEEPISODE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TitleEnglish",
                table: "AnimeEpisode");

            migrationBuilder.DropColumn(
                name: "TitleJapanese",
                table: "AnimeEpisode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TitleEnglish",
                table: "AnimeEpisode",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleJapanese",
                table: "AnimeEpisode",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
