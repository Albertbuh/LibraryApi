using Library.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.API.Infrastructure.EntityConfigurations;

public class BookAuthorTypeConfiguration : IEntityTypeConfiguration<BookAuthor>
{
  public void Configure(EntityTypeBuilder<BookAuthor> builder)
  {
    builder.ToTable("m2m_book_author");

    builder.Property(b => b.AuthorId).HasColumnName("author_id");
    builder.Property(b => b.BookId).HasColumnName("book_id");
  }
}
