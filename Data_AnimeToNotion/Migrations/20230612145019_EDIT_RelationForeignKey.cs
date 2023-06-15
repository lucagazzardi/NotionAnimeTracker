using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_AnimeToNotion.Migrations
{
    /// <inheritdoc />
    public partial class EDIT_RelationForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Relation_AnimeShow_AnimeShowId",
                table: "Relation");

            migrationBuilder.DropIndex(
                name: "IX_Relation_AnimeShowId",
                table: "Relation");

            migrationBuilder.DropColumn(
                name: "AnimeShowId",
                table: "Relation");

            migrationBuilder.CreateIndex(
                name: "IX_Relation_AnimeShowParentId",
                table: "Relation",
                column: "AnimeShowParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Relation_AnimeShow_AnimeShowParentId",
                table: "Relation",
                column: "AnimeShowParentId",
                principalTable: "AnimeShow",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Relation_AnimeShow_AnimeShowParentId",
                table: "Relation");

            migrationBuilder.DropIndex(
                name: "IX_Relation_AnimeShowParentId",
                table: "Relation");

            migrationBuilder.AddColumn<Guid>(
                name: "AnimeShowId",
                table: "Relation",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Relation_AnimeShowId",
                table: "Relation",
                column: "AnimeShowId");

            migrationBuilder.AddForeignKey(
                name: "FK_Relation_AnimeShow_AnimeShowId",
                table: "Relation",
                column: "AnimeShowId",
                principalTable: "AnimeShow",
                principalColumn: "Id");
        }
    }
}
