using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_AnimeToNotion.Migrations
{
    /// <inheritdoc />
    public partial class ADD_VariousEdits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimeShow_Saga_SagaId",
                table: "AnimeShow");

            migrationBuilder.DropTable(
                name: "Saga");

            migrationBuilder.DropIndex(
                name: "IX_AnimeShow_SagaId",
                table: "AnimeShow");

            migrationBuilder.DropColumn(
                name: "SagaId",
                table: "AnimeShow");

            migrationBuilder.DropColumn(
                name: "CompletedYear",
                table: "WatchingTime");

            migrationBuilder.AddColumn<Guid>(
                name: "CompletedYear",
                table: "WatchingTime",
                nullable: true
                );            

            migrationBuilder.CreateTable(
                name: "Year",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotionPageId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YearValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Year", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WatchingTime_CompletedYear",
                table: "WatchingTime",
                column: "CompletedYear");

            migrationBuilder.AddForeignKey(
                name: "FK_WatchingTime_Year_CompletedYear",
                table: "WatchingTime",
                column: "CompletedYear",
                principalTable: "Year",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WatchingTime_Year_CompletedYear",
                table: "WatchingTime");

            migrationBuilder.DropTable(
                name: "Year");

            migrationBuilder.DropIndex(
                name: "IX_WatchingTime_CompletedYear",
                table: "WatchingTime");

            migrationBuilder.AlterColumn<int>(
                name: "CompletedYear",
                table: "WatchingTime",
                type: "int",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "WatchingTimeId",
                table: "AnimeShow",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 12)
                .OldAnnotation("Relational:ColumnOrder", 11);

            migrationBuilder.AlterColumn<Guid>(
                name: "ScoreId",
                table: "AnimeShow",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 11)
                .OldAnnotation("Relational:ColumnOrder", 10);

            migrationBuilder.AlterColumn<Guid>(
                name: "NoteId",
                table: "AnimeShow",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 13)
                .OldAnnotation("Relational:ColumnOrder", 12);

            migrationBuilder.AddColumn<Guid>(
                name: "SagaId",
                table: "AnimeShow",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"))
                .Annotation("Relational:ColumnOrder", 10);

            migrationBuilder.CreateTable(
                name: "Saga",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Saga", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimeShow_SagaId",
                table: "AnimeShow",
                column: "SagaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeShow_Saga_SagaId",
                table: "AnimeShow",
                column: "SagaId",
                principalTable: "Saga",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
