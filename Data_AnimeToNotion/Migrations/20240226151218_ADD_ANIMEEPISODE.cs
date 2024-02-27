using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_AnimeToNotion.Migrations
{
    /// <inheritdoc />
    public partial class ADD_ANIMEEPISODE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnimeEpisode",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnimeShowId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EpisodeNumber = table.Column<int>(type: "int", nullable: false),
                    TitleEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleJapanese = table.Column<string>(type: "nvarchar(max)", nullable: true),
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

            migrationBuilder.CreateIndex(
                name: "IX_AnimeEpisode_AnimeShowId_EpisodeNumber",
                table: "AnimeEpisode",
                columns: new[] { "AnimeShowId", "EpisodeNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimeEpisode");
        }
    }
}
