using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_AnimeToNotion.Migrations
{
    /// <inheritdoc />
    public partial class EDIT_GenreStudio_Index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudioOnAnimeShow_AnimeShowId",
                table: "StudioOnAnimeShow");

            migrationBuilder.DropIndex(
                name: "IX_GenreOnAnimeShow_AnimeShowId",
                table: "GenreOnAnimeShow");

            migrationBuilder.CreateIndex(
                name: "IX_StudioOnAnimeShow_AnimeShowId_StudioId",
                table: "StudioOnAnimeShow",
                columns: new[] { "AnimeShowId", "StudioId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GenreOnAnimeShow_AnimeShowId_GenreId",
                table: "GenreOnAnimeShow",
                columns: new[] { "AnimeShowId", "GenreId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudioOnAnimeShow_AnimeShowId_StudioId",
                table: "StudioOnAnimeShow");

            migrationBuilder.DropIndex(
                name: "IX_GenreOnAnimeShow_AnimeShowId_GenreId",
                table: "GenreOnAnimeShow");

            migrationBuilder.CreateIndex(
                name: "IX_StudioOnAnimeShow_AnimeShowId",
                table: "StudioOnAnimeShow",
                column: "AnimeShowId");

            migrationBuilder.CreateIndex(
                name: "IX_GenreOnAnimeShow_AnimeShowId",
                table: "GenreOnAnimeShow",
                column: "AnimeShowId");
        }
    }
}
