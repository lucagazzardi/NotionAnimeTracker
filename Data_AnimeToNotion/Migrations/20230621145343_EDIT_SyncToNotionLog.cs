using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_AnimeToNotion.Migrations
{
    /// <inheritdoc />
    public partial class EDIT_SyncToNotionLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Message",
                table: "SyncToNotionLog",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Esito",
                table: "SyncToNotionLog",
                newName: "Result");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "SyncToNotionLog",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "Result",
                table: "SyncToNotionLog",
                newName: "Esito");
        }
    }
}
