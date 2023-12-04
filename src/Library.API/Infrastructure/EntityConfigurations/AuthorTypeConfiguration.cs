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
    
    
    builder.HasIndex(a => a.FirstName);
  }
}
