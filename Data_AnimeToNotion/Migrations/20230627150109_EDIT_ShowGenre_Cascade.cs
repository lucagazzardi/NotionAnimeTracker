using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_AnimeToNotion.Migrations
{
    /// <inheritdoc />
    public partial class EDIT_ShowGenre_Cascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GenreOnAnimeShow_Genre_GenreId",
                table: "GenreOnAnimeShow");

            migrationBuilder.DropForeignKey(
                name: "FK_StudioOnAnimeShow_Studio_StudioId",
                table: "StudioOnAnimeShow");

            migrationBuilder.AddForeignKey(
                name: "FK_GenreOnAnimeShow_Genre_GenreId",
                table: "GenreOnAnimeShow",
                column: "GenreId",
                principalTable: "Genre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudioOnAnimeShow_Studio_StudioId",
                table: "StudioOnAnimeShow",
                column: "StudioId",
                principalTable: "Studio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GenreOnAnimeShow_Genre_GenreId",
                table: "GenreOnAnimeShow");

            migrationBuilder.DropForeignKey(
                name: "FK_StudioOnAnimeShow_Studio_StudioId",
                table: "StudioOnAnimeShow");

            migrationBuilder.AddForeignKey(
                name: "FK_GenreOnAnimeShow_Genre_GenreId",
                table: "GenreOnAnimeShow",
                column: "GenreId",
                principalTable: "Genre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudioOnAnimeShow_Studio_StudioId",
                table: "StudioOnAnimeShow",
                column: "StudioId",
                principalTable: "Studio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
