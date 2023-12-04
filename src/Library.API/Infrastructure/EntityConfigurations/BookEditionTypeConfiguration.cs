using Library.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.API.Infrastructure.EntityConfigurations;

public class BookEditionTypeConfiguration : IEntityTypeConfiguration<BookEdition>
{
  public void Configure(EntityTypeBuilder<BookEdition> builder)
  {
    builder.ToTable("book_editions");

    builder.Property(be => be.Id).HasColumnName("be_id");
    
    builder
      .Property(be => be.ISBN)
      .HasColumnName("be_isbn")
      .HasColumnType("varchar(20)")
      .HasCharSet("utf8mb4")
      .UseCollation("utf8mb4_general_ci")
      .IsRequired();

    builder
      .Property(be => be.Title)
      .HasColumnName("be_title")
      .HasColumnType("varchar(255)")
      .HasCharSet("utf8mb4")
      .UseCollation("utf8mb4_general_ci")
      .IsRequired();  

    builder.Property(be => be.Description).HasColumnName("be_description");

    builder.HasIndex(be => be.ISBN).IsUnique();
    
    builder.HasMany(be => be.Authors)
      .WithMany(a => a.BookEditions)
      .UsingEntity(
          "m2m_editions_authors",
          l => l.HasOne(typeof(Author)).WithMany().HasForeignKey("author_id"),
          r => r.HasOne(typeof(BookEdition)).WithMany().HasForeignKey("edition_id"),
          j => j.HasKey("author_id", "edition_id")
          );
    
    builder.HasMany(be => be.Genres)
      .WithMany(g => g.BookEditions)
      .UsingEntity(
        "m2m_editions_genres",
        l => l.HasOne(typeof(Genre)).WithMany().HasForeignKey("genre_id"),
        r => r.HasOne(typeof(BookEdition)).WithMany().HasForeignKey("edition_id"),
        j => j.HasKey("genre_id", "edition_id")
      );

  }
}
