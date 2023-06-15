using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_AnimeToNotion.Migrations
{
    /// <inheritdoc />
    public partial class ADDED_Relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CompletedYear",
                table: "WatchingTime",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "Favorite",
                table: "Score",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "NotionPageId",
                table: "AnimeShow",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Relation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnimeMalId = table.Column<int>(type: "int", nullable: false),
                    AnimeRelatedMalId = table.Column<int>(type: "int", nullable: false),
                    RelationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnimeShowParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AnimeShowId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Relation_AnimeShow_AnimeShowId",
                        column: x => x.AnimeShowId,
                        principalTable: "AnimeShow",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimeShow_MalId",
                table: "AnimeShow",
                column: "MalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimeShow_NotionPageId",
                table: "AnimeShow",
                column: "NotionPageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Relation_AnimeShowId",
                table: "Relation",
                column: "AnimeShowId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Relation");

            migrationBuilder.DropIndex(
                name: "IX_AnimeShow_MalId",
                table: "AnimeShow");

            migrationBuilder.DropIndex(
                name: "IX_AnimeShow_NotionPageId",
                table: "AnimeShow");

            migrationBuilder.AlterColumn<int>(
                name: "CompletedYear",
                table: "WatchingTime",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Favorite",
                table: "Score",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NotionPageId",
                table: "AnimeShow",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
