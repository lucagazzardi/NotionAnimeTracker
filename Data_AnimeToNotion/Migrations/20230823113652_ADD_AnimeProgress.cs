using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_AnimeToNotion.Migrations
{
    /// <inheritdoc />
    public partial class ADD_AnimeProgress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimeShow_Note_NoteId",
                table: "AnimeShow");

            migrationBuilder.DropForeignKey(
                name: "FK_AnimeShow_Score_ScoreId",
                table: "AnimeShow");

            migrationBuilder.DropForeignKey(
                name: "FK_AnimeShow_WatchingTime_WatchingTimeId",
                table: "AnimeShow");

            migrationBuilder.DropTable(
                name: "Note");

            migrationBuilder.DropTable(
                name: "Score");

            migrationBuilder.DropTable(
                name: "SyncToNotionLog");

            migrationBuilder.DropTable(
                name: "WatchingTime");

            migrationBuilder.DropIndex(
                name: "IX_AnimeShow_NoteId",
                table: "AnimeShow");

            migrationBuilder.DropIndex(
                name: "IX_AnimeShow_ScoreId",
                table: "AnimeShow");

            migrationBuilder.DropIndex(
                name: "IX_AnimeShow_WatchingTimeId",
                table: "AnimeShow");

            migrationBuilder.DropColumn(
                name: "NoteId",
                table: "AnimeShow");

            migrationBuilder.DropColumn(
                name: "ScoreId",
                table: "AnimeShow");

            migrationBuilder.DropColumn(
                name: "WatchingTimeId",
                table: "AnimeShow");

            migrationBuilder.AlterColumn<string>(
                name: "NotionPageId",
                table: "Year",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "StudioId",
                table: "StudioOnAnimeShow",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "AnimeShowId",
                table: "StudioOnAnimeShow",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "StudioOnAnimeShow",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<int>(
                name: "MalId",
                table: "Studio",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Studio",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Studio",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<string>(
                name: "RelationType",
                table: "Relation",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true)
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<Guid>(
                name: "AnimeShowId",
                table: "Relation",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "AnimeRelatedMalId",
                table: "Relation",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Relation",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<string>(
                name: "NotionPageId",
                table: "NotionSync",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "GenreId",
                table: "GenreOnAnimeShow",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "AnimeShowId",
                table: "GenreOnAnimeShow",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "GenreOnAnimeShow",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "AnimeShow",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "AnimeShow",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AnimeShowProgress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnimeShowId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonalScore = table.Column<int>(type: "int", nullable: true),
                    StartedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinishedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedYear = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeShowProgress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimeShowProgress_AnimeShow_AnimeShowId",
                        column: x => x.AnimeShowId,
                        principalTable: "AnimeShow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Year_NotionPageId",
                table: "Year",
                column: "NotionPageId");

            migrationBuilder.CreateIndex(
                name: "IX_Relation_AnimeRelatedMalId",
                table: "Relation",
                column: "AnimeRelatedMalId");

            migrationBuilder.CreateIndex(
                name: "IX_NotionSync_NotionPageId",
                table: "NotionSync",
                column: "NotionPageId");

            migrationBuilder.CreateIndex(
                name: "IX_Genre_MalId",
                table: "Genre",
                column: "MalId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeShow_Status",
                table: "AnimeShow",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeShowProgress_AnimeShowId",
                table: "AnimeShowProgress",
                column: "AnimeShowId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimeShowProgress");

            migrationBuilder.DropIndex(
                name: "IX_Year_NotionPageId",
                table: "Year");

            migrationBuilder.DropIndex(
                name: "IX_Relation_AnimeRelatedMalId",
                table: "Relation");

            migrationBuilder.DropIndex(
                name: "IX_NotionSync_NotionPageId",
                table: "NotionSync");

            migrationBuilder.DropIndex(
                name: "IX_Genre_MalId",
                table: "Genre");

            migrationBuilder.DropIndex(
                name: "IX_AnimeShow_Status",
                table: "AnimeShow");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "AnimeShow");

            migrationBuilder.AlterColumn<string>(
                name: "NotionPageId",
                table: "Year",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "StudioId",
                table: "StudioOnAnimeShow",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "AnimeShowId",
                table: "StudioOnAnimeShow",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "StudioOnAnimeShow",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<int>(
                name: "MalId",
                table: "Studio",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Studio",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Studio",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<string>(
                name: "RelationType",
                table: "Relation",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<Guid>(
                name: "AnimeShowId",
                table: "Relation",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "AnimeRelatedMalId",
                table: "Relation",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Relation",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<string>(
                name: "NotionPageId",
                table: "NotionSync",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "GenreId",
                table: "GenreOnAnimeShow",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "AnimeShowId",
                table: "GenreOnAnimeShow",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "GenreOnAnimeShow",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "AnimeShow",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NoteId",
                table: "AnimeShow",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ScoreId",
                table: "AnimeShow",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WatchingTimeId",
                table: "AnimeShow",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Note",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Score",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MalScore = table.Column<int>(type: "int", nullable: false),
                    PersonalScore = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Score", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SyncToNotionLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MalId = table.Column<int>(type: "int", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncToNotionLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WatchingTime",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompletedYear = table.Column<int>(type: "int", nullable: true),
                    FinishedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchingTime", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimeShow_NoteId",
                table: "AnimeShow",
                column: "NoteId",
                unique: true,
                filter: "[NoteId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeShow_ScoreId",
                table: "AnimeShow",
                column: "ScoreId",
                unique: true,
                filter: "[ScoreId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeShow_WatchingTimeId",
                table: "AnimeShow",
                column: "WatchingTimeId",
                unique: true,
                filter: "[WatchingTimeId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeShow_Note_NoteId",
                table: "AnimeShow",
                column: "NoteId",
                principalTable: "Note",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeShow_Score_ScoreId",
                table: "AnimeShow",
                column: "ScoreId",
                principalTable: "Score",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeShow_WatchingTime_WatchingTimeId",
                table: "AnimeShow",
                column: "WatchingTimeId",
                principalTable: "WatchingTime",
                principalColumn: "Id");
        }
    }
}
