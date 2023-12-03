using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class renamingMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TakenBooks_Books_BookId",
                table: "TakenBooks");

            migrationBuilder.DropTable(
                name: "AuthorBookEdition");

            migrationBuilder.DropTable(
                name: "BookEditionGenre");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Genres",
                table: "Genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Authors",
                table: "Authors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TakenBooks",
                table: "TakenBooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Books",
                table: "Books");

            migrationBuilder.RenameTable(
                name: "Genres",
                newName: "genres");

            migrationBuilder.RenameTable(
                name: "Authors",
                newName: "authors");

            migrationBuilder.RenameTable(
                name: "TakenBooks",
                newName: "book_instances");

            migrationBuilder.RenameTable(
                name: "Books",
                newName: "book_editions");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "genres",
                newName: "g_name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "genres",
                newName: "g_id");

            migrationBuilder.RenameColumn(
                name: "MiddleName",
                table: "authors",
                newName: "a_middlename");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "authors",
                newName: "a_lastname");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "authors",
                newName: "a_firstname");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "authors",
                newName: "a_id");

            migrationBuilder.RenameColumn(
                name: "DateOfTaken",
                table: "book_instances",
                newName: "bi_date_of_taken");

            migrationBuilder.RenameColumn(
                name: "DateOfReturn",
                table: "book_instances",
                newName: "bi_date_of_return");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "book_instances",
                newName: "bi_id");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "book_instances",
                newName: "bi_book_edition_id");

            migrationBuilder.RenameIndex(
                name: "IX_TakenBooks_BookId",
                table: "book_instances",
                newName: "IX_book_instances_bi_book_edition_id");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "book_editions",
                newName: "be_title");

            migrationBuilder.RenameColumn(
                name: "ISBN",
                table: "book_editions",
                newName: "be_isbn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "book_editions",
                newName: "be_description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "book_editions",
                newName: "be_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_genres",
                table: "genres",
                column: "g_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_authors",
                table: "authors",
                column: "a_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_book_instances",
                table: "book_instances",
                column: "bi_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_book_editions",
                table: "book_editions",
                column: "be_id");

            migrationBuilder.CreateTable(
                name: "m2m_editions_authors",
                columns: table => new
                {
                    authorid = table.Column<int>(name: "author_id", type: "int", nullable: false),
                    editionid = table.Column<int>(name: "edition_id", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m2m_editions_authors", x => new { x.authorid, x.editionid });
                    table.ForeignKey(
                        name: "FK_m2m_editions_authors_authors_author_id",
                        column: x => x.authorid,
                        principalTable: "authors",
                        principalColumn: "a_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_m2m_editions_authors_book_editions_edition_id",
                        column: x => x.editionid,
                        principalTable: "book_editions",
                        principalColumn: "be_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "m2m_editions_genres",
                columns: table => new
                {
                    genreid = table.Column<int>(name: "genre_id", type: "int", nullable: false),
                    editionid = table.Column<int>(name: "edition_id", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m2m_editions_genres", x => new { x.genreid, x.editionid });
                    table.ForeignKey(
                        name: "FK_m2m_editions_genres_book_editions_edition_id",
                        column: x => x.editionid,
                        principalTable: "book_editions",
                        principalColumn: "be_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_m2m_editions_genres_genres_genre_id",
                        column: x => x.genreid,
                        principalTable: "genres",
                        principalColumn: "g_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_m2m_editions_authors_edition_id",
                table: "m2m_editions_authors",
                column: "edition_id");

            migrationBuilder.CreateIndex(
                name: "IX_m2m_editions_genres_edition_id",
                table: "m2m_editions_genres",
                column: "edition_id");

            migrationBuilder.AddForeignKey(
                name: "FK_book_instances_book_editions_bi_book_edition_id",
                table: "book_instances",
                column: "bi_book_edition_id",
                principalTable: "book_editions",
                principalColumn: "be_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_book_instances_book_editions_bi_book_edition_id",
                table: "book_instances");

            migrationBuilder.DropTable(
                name: "m2m_editions_authors");

            migrationBuilder.DropTable(
                name: "m2m_editions_genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_genres",
                table: "genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_authors",
                table: "authors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_book_instances",
                table: "book_instances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_book_editions",
                table: "book_editions");

            migrationBuilder.RenameTable(
                name: "genres",
                newName: "Genres");

            migrationBuilder.RenameTable(
                name: "authors",
                newName: "Authors");

            migrationBuilder.RenameTable(
                name: "book_instances",
                newName: "TakenBooks");

            migrationBuilder.RenameTable(
                name: "book_editions",
                newName: "Books");

            migrationBuilder.RenameColumn(
                name: "g_name",
                table: "Genres",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "g_id",
                table: "Genres",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "a_middlename",
                table: "Authors",
                newName: "MiddleName");

            migrationBuilder.RenameColumn(
                name: "a_lastname",
                table: "Authors",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "a_firstname",
                table: "Authors",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "a_id",
                table: "Authors",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "bi_date_of_taken",
                table: "TakenBooks",
                newName: "DateOfTaken");

            migrationBuilder.RenameColumn(
                name: "bi_date_of_return",
                table: "TakenBooks",
                newName: "DateOfReturn");

            migrationBuilder.RenameColumn(
                name: "bi_id",
                table: "TakenBooks",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "bi_book_edition_id",
                table: "TakenBooks",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_book_instances_bi_book_edition_id",
                table: "TakenBooks",
                newName: "IX_TakenBooks_BookId");

            migrationBuilder.RenameColumn(
                name: "be_title",
                table: "Books",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "be_isbn",
                table: "Books",
                newName: "ISBN");

            migrationBuilder.RenameColumn(
                name: "be_description",
                table: "Books",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "be_id",
                table: "Books",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genres",
                table: "Genres",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Authors",
                table: "Authors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TakenBooks",
                table: "TakenBooks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Books",
                table: "Books",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AuthorBookEdition",
                columns: table => new
                {
                    AuthorsId = table.Column<int>(type: "int", nullable: false),
                    BooksId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorBookEdition", x => new { x.AuthorsId, x.BooksId });
                    table.ForeignKey(
                        name: "FK_AuthorBookEdition_Authors_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorBookEdition_Books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BookEditionGenre",
                columns: table => new
                {
                    BooksId = table.Column<int>(type: "int", nullable: false),
                    GenresId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookEditionGenre", x => new { x.BooksId, x.GenresId });
                    table.ForeignKey(
                        name: "FK_BookEditionGenre_Books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookEditionGenre_Genres_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorBookEdition_BooksId",
                table: "AuthorBookEdition",
                column: "BooksId");

            migrationBuilder.CreateIndex(
                name: "IX_BookEditionGenre_GenresId",
                table: "BookEditionGenre",
                column: "GenresId");

            migrationBuilder.AddForeignKey(
                name: "FK_TakenBooks_Books_BookId",
                table: "TakenBooks",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
