using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_AnimeToNotion.Migrations
{
    /// <inheritdoc />
    public partial class ADD_ForeignAnimeShow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Relation_AnimeShow_AnimeShowId",
                table: "Relation");

            migrationBuilder.DropColumn(
                name: "AnimeMalId",
                table: "Relation");

            migrationBuilder.AlterColumn<Guid>(
                name: "AnimeShowId",
                table: "Relation",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddForeignKey(
                name: "FK_Relation_AnimeShow_AnimeShowId",
                table: "Relation",
                column: "AnimeShowId",
                principalTable: "AnimeShow",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Relation_AnimeShow_AnimeShowId",
                table: "Relation");

            migrationBuilder.AlterColumn<Guid>(
                name: "AnimeShowId",
                table: "Relation",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<int>(
                name: "AnimeMalId",
                table: "Relation",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddForeignKey(
                name: "FK_Relation_AnimeShow_AnimeShowId",
                table: "Relation",
                column: "AnimeShowId",
                principalTable: "AnimeShow",
                principalColumn: "Id");
        }
    }
}
