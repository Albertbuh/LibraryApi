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
  }
}
