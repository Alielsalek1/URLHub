using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace URLshortner.Migrations
{
    /// <inheritdoc />
    public partial class dbv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_URLS",
                table: "URLS");

            migrationBuilder.RenameTable(
                name: "URLS",
                newName: "URLs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_URLs",
                table: "URLs",
                columns: new[] { "ID", "Url" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_URLs",
                table: "URLs");

            migrationBuilder.RenameTable(
                name: "URLs",
                newName: "URLS");

            migrationBuilder.AddPrimaryKey(
                name: "PK_URLS",
                table: "URLS",
                columns: new[] { "ID", "Url" });
        }
    }
}
