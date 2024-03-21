using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_AnimeToNotion.Migrations
{
    /// <inheritdoc />
    public partial class Index_AnimeEpisode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AnimeEpisode_AnimeShowId",
                table: "AnimeEpisode");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeEpisode_AnimeShowId",
                table: "AnimeEpisode",
                column: "AnimeShowId")
                .Annotation("SqlServer:Include", new[] { "EpisodeNumber", "WatchedOn" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AnimeEpisode_AnimeShowId",
                table: "AnimeEpisode");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeEpisode_AnimeShowId",
                table: "AnimeEpisode",
                column: "AnimeShowId");
        }
    }
}
