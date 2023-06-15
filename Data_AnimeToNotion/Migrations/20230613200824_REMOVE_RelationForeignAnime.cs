using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_AnimeToNotion.Migrations
{
    /// <inheritdoc />
    public partial class REMOVE_RelationForeignAnime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Relation_AnimeShow_AnimeShowParentId",
                table: "Relation");

            migrationBuilder.RenameColumn(
                name: "AnimeShowParentId",
                table: "Relation",
                newName: "AnimeShowId");

            migrationBuilder.RenameIndex(
                name: "IX_Relation_AnimeShowParentId",
                table: "Relation",
                newName: "IX_Relation_AnimeShowId");

            migrationBuilder.AlterColumn<Guid>(
                name: "AnimeShowId",
                table: "Relation",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true)
                .OldAnnotation("Relational:ColumnOrder", 4);

            migrationBuilder.AddForeignKey(
                name: "FK_Relation_AnimeShow_AnimeShowId",
                table: "Relation",
                column: "AnimeShowId",
                principalTable: "AnimeShow",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Relation_AnimeShow_AnimeShowId",
                table: "Relation");

            migrationBuilder.RenameColumn(
                name: "AnimeShowId",
                table: "Relation",
                newName: "AnimeShowParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Relation_AnimeShowId",
                table: "Relation",
                newName: "IX_Relation_AnimeShowParentId");

            migrationBuilder.AlterColumn<Guid>(
                name: "AnimeShowParentId",
                table: "Relation",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 4);

            migrationBuilder.AddForeignKey(
                name: "FK_Relation_AnimeShow_AnimeShowParentId",
                table: "Relation",
                column: "AnimeShowParentId",
                principalTable: "AnimeShow",
                principalColumn: "Id");
        }
    }
}
