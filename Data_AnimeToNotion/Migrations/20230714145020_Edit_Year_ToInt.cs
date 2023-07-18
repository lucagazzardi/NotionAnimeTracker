using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_AnimeToNotion.Migrations
{
    /// <inheritdoc />
    public partial class Edit_Year_ToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WatchingTime_Year_CompletedYear",
                table: "WatchingTime");

            migrationBuilder.DropIndex(
                name: "IX_WatchingTime_CompletedYear",
                table: "WatchingTime");

            migrationBuilder.DropColumn(
                name: "CompletedYear",
                table: "WatchingTime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartedOn",
                table: "WatchingTime",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FinishedOn",
                table: "WatchingTime",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "WatchingTime",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .OldAnnotation("Relational:ColumnOrder", 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartedOn",
                table: "WatchingTime",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FinishedOn",
                table: "WatchingTime",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "WatchingTime",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddColumn<Guid>(
                name: "CompletedYear",
                table: "WatchingTime",
                type: "uniqueidentifier",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 3);

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
    }
}
