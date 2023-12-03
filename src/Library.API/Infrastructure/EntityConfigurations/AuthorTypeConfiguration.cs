using Library.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.API.Infrastructure.EntityConfigurations;

public class AuthorTypeConfiguration : IEntityTypeConfiguration<Author>
{
  public void Configure(EntityTypeBuilder<Author> builder)
  {
    builder.ToTable("Authors");

    builder
      .Property(a => a.FirstName)
      .HasColumnType("varchar(255)")
      .HasCharSet("utf8mb4")
      .UseCollation("utf8mb4_general_ci")
      .IsRequired();
    builder
      .Property(a => a.MiddleName)
      .HasColumnType("varchar(255)")
      .HasCharSet("utf8mb4")
      .UseCollation("utf8mb4_general_ci");
    builder
      .Property(a => a.LastName)
      .HasColumnType("varchar(255)")
      .HasCharSet("utf8mb4")
      .UseCollation("utf8mb4_general_ci");
  }
}
