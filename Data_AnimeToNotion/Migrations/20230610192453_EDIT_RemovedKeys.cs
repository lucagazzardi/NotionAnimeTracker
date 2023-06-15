using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_AnimeToNotion.Migrations
{
    /// <inheritdoc />
    public partial class EDIT_RemovedKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Note_AnimeShow_AnimeShowId",
                table: "Note");

            migrationBuilder.DropForeignKey(
                name: "FK_Score_AnimeShow_AnimeShowId",
                table: "Score");

            migrationBuilder.DropForeignKey(
                name: "FK_WatchingTime_AnimeShow_AnimeShowId",
                table: "WatchingTime");

            migrationBuilder.DropIndex(
                name: "IX_WatchingTime_AnimeShowId",
                table: "WatchingTime");

            migrationBuilder.DropIndex(
                name: "IX_Score_AnimeShowId",
                table: "Score");

            migrationBuilder.DropIndex(
                name: "IX_Note_AnimeShowId",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "AnimeShowId",
                table: "WatchingTime");

            migrationBuilder.DropColumn(
                name: "AnimeShowId",
                table: "Score");

            migrationBuilder.DropColumn(
                name: "AnimeShowId",
                table: "Note");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartedOn",
                table: "WatchingTime",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2")
                .Annotation("Relational:ColumnOrder", 1)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FinishedOn",
                table: "WatchingTime",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 2)
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<int>(
                name: "CompletedYear",
                table: "WatchingTime",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 3)
                .OldAnnotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<int>(
                name: "PersonalScore",
                table: "Score",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 2)
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<int>(
                name: "MalScore",
                table: "Score",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 1)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<bool>(
                name: "Favorite",
                table: "Score",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit")
                .Annotation("Relational:ColumnOrder", 3)
                .OldAnnotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Note",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)")
                .Annotation("Relational:ColumnOrder", 1)
                .OldAnnotation("Relational:ColumnOrder", 2);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_AnimeShow_NoteId",
                table: "AnimeShow");

            migrationBuilder.DropIndex(
                name: "IX_AnimeShow_ScoreId",
                table: "AnimeShow");

            migrationBuilder.DropIndex(
                name: "IX_AnimeShow_WatchingTimeId",
                table: "AnimeShow");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartedOn",
                table: "WatchingTime",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2")
                .Annotation("Relational:ColumnOrder", 2)
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FinishedOn",
                table: "WatchingTime",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 3)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "CompletedYear",
                table: "WatchingTime",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 4)
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AddColumn<Guid>(
                name: "AnimeShowId",
                table: "WatchingTime",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"))
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "PersonalScore",
                table: "Score",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 3)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<int>(
                name: "MalScore",
                table: "Score",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 2)
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<bool>(
                name: "Favorite",
                table: "Score",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit")
                .Annotation("Relational:ColumnOrder", 4)
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AddColumn<Guid>(
                name: "AnimeShowId",
                table: "Score",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"))
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Note",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)")
                .Annotation("Relational:ColumnOrder", 2)
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<Guid>(
                name: "AnimeShowId",
                table: "Note",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"))
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.CreateIndex(
                name: "IX_WatchingTime_AnimeShowId",
                table: "WatchingTime",
                column: "AnimeShowId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Score_AnimeShowId",
                table: "Score",
                column: "AnimeShowId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Note_AnimeShowId",
                table: "Note",
                column: "AnimeShowId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Note_AnimeShow_AnimeShowId",
                table: "Note",
                column: "AnimeShowId",
                principalTable: "AnimeShow",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Score_AnimeShow_AnimeShowId",
                table: "Score",
                column: "AnimeShowId",
                principalTable: "AnimeShow",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WatchingTime_AnimeShow_AnimeShowId",
                table: "WatchingTime",
                column: "AnimeShowId",
                principalTable: "AnimeShow",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
