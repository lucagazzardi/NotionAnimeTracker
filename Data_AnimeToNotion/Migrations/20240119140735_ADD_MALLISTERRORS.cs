using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_AnimeToNotion.Migrations
{
    /// <inheritdoc />
    public partial class ADD_MALLISTERRORS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MalListToSync",
                table: "NotionSync",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "MalSyncError",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimeShowId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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

            migrationBuilder.CreateIndex(
                name: "IX_MalSyncError_AnimeShowId",
                table: "MalSyncError",
                column: "AnimeShowId");

            migrationBuilder.CreateIndex(
                name: "IX_MalSyncError_MalId",
                table: "MalSyncError",
                column: "MalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MalSyncError");

            migrationBuilder.DropColumn(
                name: "MalListToSync",
                table: "NotionSync");
        }
    }
}
