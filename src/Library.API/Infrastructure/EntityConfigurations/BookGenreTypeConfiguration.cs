using Library.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.API.Infrastructure.EntityConfigurations;

public class BookGenreTypeConfiguration : IEntityTypeConfiguration<BookGenre>
{
  public void Configure(EntityTypeBuilder<BookGenre> builder)
  {
    builder.ToTable("m2m_book_genre");

    builder.Property(b => b.GenreId).HasColumnName("genre_id");
    builder.Property(b => b.BookId).HasColumnName("book_id");
  }
}
