using Library.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.API.Infrastructure.EntityConfigurations;

public class BookTypeConfiguration : IEntityTypeConfiguration<BookEdition>
{
  public void Configure(EntityTypeBuilder<BookEdition> builder)
  {
    builder.ToTable("Books");

    builder
      .Property(b => b.ISBN)
      .HasColumnType("varchar(20)")
      .HasCharSet("utf8mb4")
      .UseCollation("utf8mb4_general_ci")
      .IsRequired();

    builder
      .Property(b => b.Title)
      .HasColumnType("varchar(255)")
      .HasCharSet("utf8mb4")
      .UseCollation("utf8mb4_general_ci")
      .IsRequired();  
  }
}
