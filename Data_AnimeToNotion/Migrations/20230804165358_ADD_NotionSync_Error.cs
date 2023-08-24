using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_AnimeToNotion.Migrations
{
    /// <inheritdoc />
    public partial class ADD_NotionSync_Error : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Error",
                table: "NotionSync",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Error",
                table: "NotionSync");
        }
    }
}
