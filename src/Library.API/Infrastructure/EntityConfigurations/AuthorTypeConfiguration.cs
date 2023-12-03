using Library.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.API.Infrastructure.EntityConfigurations;

public class AuthorTypeConfiguration : IEntityTypeConfiguration<Author>
{
  public void Configure(EntityTypeBuilder<Author> builder)
  {
    builder.ToTable("authors");

    builder.Property(a => a.Id).HasColumnName("a_id");
    builder
      .Property(a => a.FirstName)
      .HasColumnName("a_firstname")
      .HasColumnType("varchar(255)")
      .HasCharSet("utf8mb4")
      .UseCollation("utf8mb4_general_ci")
      .IsRequired();
    builder
      .Property(a => a.MiddleName)
      .HasColumnName("a_middlename")
      .HasColumnType("varchar(255)")
      .HasCharSet("utf8mb4")
      .UseCollation("utf8mb4_general_ci");
    builder
      .Property(a => a.LastName)
      .HasColumnName("a_lastname")
      .HasColumnType("varchar(255)")
      .HasCharSet("utf8mb4")
      .UseCollation("utf8mb4_general_ci");
    
    builder.HasMany(a => a.BookEditions)
      .WithMany(be => be.Authors)
      .UsingEntity(
          "m2m_editions_authors",
          l => l.HasOne(typeof(BookEdition)).WithMany().HasForeignKey("edition_id"),
          r => r.HasOne(typeof(Author)).WithMany().HasForeignKey("author_id"),
          j => j.HasKey("author_id", "edition_id")
          );

    builder.HasIndex(a => a.FirstName);
  }
  
    
}
