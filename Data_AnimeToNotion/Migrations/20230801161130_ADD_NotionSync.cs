using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_AnimeToNotion.Migrations
{
    /// <inheritdoc />
    public partial class ADD_NotionSync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotionSync",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimeShowId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NotionPageId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToSync = table.Column<bool>(type: "bit", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_NotionSync_AnimeShowId",
                table: "NotionSync",
                column: "AnimeShowId",
                unique: true,
                filter: "[AnimeShowId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotionSync");
        }
    }
}
