using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_AnimeToNotion.Migrations
{
    /// <inheritdoc />
    public partial class EDIT_AnimeShow_RemovedNotionPageII : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AnimeShow_NotionPageId",
                table: "AnimeShow");

            migrationBuilder.DropColumn(
                name: "NotionPageId",
                table: "AnimeShow");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NotionPageId",
                table: "AnimeShow",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimeShow_NotionPageId",
                table: "AnimeShow",
                column: "NotionPageId",
                unique: true,
                filter: "[NotionPageId] IS NOT NULL");
        }
    }
}
