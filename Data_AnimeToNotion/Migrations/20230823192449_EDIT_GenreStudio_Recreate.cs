using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_AnimeToNotion.Migrations
{
    /// <inheritdoc />
    public partial class EDIT_GenreStudio_Recreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Studio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studio", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GenreOnAnimeShow",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnimeShowId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenreOnAnimeShow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GenreOnAnimeShow_AnimeShow_AnimeShowId",
                        column: x => x.AnimeShowId,
                        principalTable: "AnimeShow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenreOnAnimeShow_Genre_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudioOnAnimeShow",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnimeShowId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudioId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudioOnAnimeShow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudioOnAnimeShow_AnimeShow_AnimeShowId",
                        column: x => x.AnimeShowId,
                        principalTable: "AnimeShow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudioOnAnimeShow_Studio_StudioId",
                        column: x => x.StudioId,
                        principalTable: "Studio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GenreOnAnimeShow_AnimeShowId_GenreId",
                table: "GenreOnAnimeShow",
                columns: new[] { "AnimeShowId", "GenreId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GenreOnAnimeShow_GenreId",
                table: "GenreOnAnimeShow",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_StudioOnAnimeShow_AnimeShowId_StudioId",
                table: "StudioOnAnimeShow",
                columns: new[] { "AnimeShowId", "StudioId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudioOnAnimeShow_StudioId",
                table: "StudioOnAnimeShow",
                column: "StudioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenreOnAnimeShow");

            migrationBuilder.DropTable(
                name: "StudioOnAnimeShow");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropTable(
                name: "Studio");
        }
    }
}
