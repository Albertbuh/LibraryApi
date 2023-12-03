using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class indexMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "g_name",
                table: "genres",
                type: "nvarchar(200)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldCollation: "utf8mb4_general_ci")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_book_editions_be_isbn",
                table: "book_editions",
                column: "be_isbn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_authors_a_firstname",
                table: "authors",
                column: "a_firstname");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_book_editions_be_isbn",
                table: "book_editions");

            migrationBuilder.DropIndex(
                name: "IX_authors_a_firstname",
                table: "authors");

            migrationBuilder.AlterColumn<string>(
                name: "g_name",
                table: "genres",
                type: "varchar(200)",
                nullable: false,
                collation: "utf8mb4_general_ci",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
