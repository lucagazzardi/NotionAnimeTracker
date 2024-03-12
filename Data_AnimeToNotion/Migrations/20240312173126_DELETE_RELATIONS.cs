using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_AnimeToNotion.Migrations
{
    /// <inheritdoc />
    public partial class DELETE_RELATIONS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Relation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Relation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnimeShowId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnimeRelatedMalId = table.Column<int>(type: "int", nullable: false),
                    Cover = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelationType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Relation_AnimeShow_AnimeShowId",
                        column: x => x.AnimeShowId,
                        principalTable: "AnimeShow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Relation_AnimeRelatedMalId",
                table: "Relation",
                column: "AnimeRelatedMalId");

            migrationBuilder.CreateIndex(
                name: "IX_Relation_AnimeShowId",
                table: "Relation",
                column: "AnimeShowId");
        }
    }
}
