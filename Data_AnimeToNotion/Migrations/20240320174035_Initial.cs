using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_AnimeToNotion.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnimeShow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MalId = table.Column<int>(type: "int", nullable: false),
                    NameDefault = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameJapanese = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Format = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Episodes = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanToWatch = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Favorite = table.Column<bool>(type: "bit", nullable: false),
                    StartedAiring = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Cover = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Score = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeShow", x => x.Id);
                });

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
                name: "Year",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotionPageId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Year", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnimeEpisode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimeShowId = table.Column<int>(type: "int", nullable: false),
                    EpisodeNumber = table.Column<int>(type: "int", nullable: false),
                    WatchedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeEpisode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimeEpisode_AnimeShow_AnimeShowId",
                        column: x => x.AnimeShowId,
                        principalTable: "AnimeShow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimeShowProgress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimeShowId = table.Column<int>(type: "int", nullable: false),
                    PersonalScore = table.Column<int>(type: "int", nullable: true),
                    StartedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinishedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedYear = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeShowProgress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimeShowProgress_AnimeShow_AnimeShowId",
                        column: x => x.AnimeShowId,
                        principalTable: "AnimeShow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MalSyncError",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimeShowId = table.Column<int>(type: "int", nullable: true),
                    MalId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MalSyncError", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MalSyncError_AnimeShow_AnimeShowId",
                        column: x => x.AnimeShowId,
                        principalTable: "AnimeShow",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NotionSync",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimeShowId = table.Column<int>(type: "int", nullable: true),
                    NotionPageId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToSync = table.Column<bool>(type: "bit", nullable: false),
                    MalListToSync = table.Column<bool>(type: "bit", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotionSync", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotionSync_AnimeShow_AnimeShowId",
                        column: x => x.AnimeShowId,
                        principalTable: "AnimeShow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "GenreOnAnimeShow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimeShowId = table.Column<int>(type: "int", nullable: false),
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimeShowId = table.Column<int>(type: "int", nullable: false),
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
                name: "IX_AnimeEpisode_AnimeShowId",
                table: "AnimeEpisode",
                column: "AnimeShowId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeShowProgress_AnimeShowId",
                table: "AnimeShowProgress",
                column: "AnimeShowId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GenreOnAnimeShow_AnimeShowId",
                table: "GenreOnAnimeShow",
                column: "AnimeShowId")
                .Annotation("SqlServer:Include", new[] { "GenreId", "Description" });

            migrationBuilder.CreateIndex(
                name: "IX_GenreOnAnimeShow_GenreId",
                table: "GenreOnAnimeShow",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_MalSyncError_AnimeShowId",
                table: "MalSyncError",
                column: "AnimeShowId");

            migrationBuilder.CreateIndex(
                name: "IX_NotionSync_AnimeShowId",
                table: "NotionSync",
                column: "AnimeShowId",
                unique: true,
                filter: "[AnimeShowId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StudioOnAnimeShow_AnimeShowId",
                table: "StudioOnAnimeShow",
                column: "AnimeShowId")
                .Annotation("SqlServer:Include", new[] { "StudioId", "Description" });

            migrationBuilder.CreateIndex(
                name: "IX_StudioOnAnimeShow_StudioId",
                table: "StudioOnAnimeShow",
                column: "StudioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimeEpisode");

            migrationBuilder.DropTable(
                name: "AnimeShowProgress");

            migrationBuilder.DropTable(
                name: "GenreOnAnimeShow");

            migrationBuilder.DropTable(
                name: "MalSyncError");

            migrationBuilder.DropTable(
                name: "NotionSync");

            migrationBuilder.DropTable(
                name: "StudioOnAnimeShow");

            migrationBuilder.DropTable(
                name: "Year");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropTable(
                name: "AnimeShow");

            migrationBuilder.DropTable(
                name: "Studio");
        }
    }
}
