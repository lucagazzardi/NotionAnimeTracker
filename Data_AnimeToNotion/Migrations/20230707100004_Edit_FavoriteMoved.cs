using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_AnimeToNotion.Migrations
{
    /// <inheritdoc />
    public partial class Edit_FavoriteMoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Favorite",
                table: "Score");

            migrationBuilder.AddColumn<bool>(
                name: "Favorite",
                table: "AnimeShow",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Favorite",
                table: "AnimeShow");

            migrationBuilder.AddColumn<bool>(
                name: "Favorite",
                table: "Score",
                type: "bit",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 3);
        }
    }
}
