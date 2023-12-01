using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Library.API.Models;
namespace Library.API.Infrastructure.EntityConfigurations;

public class AuthorTypeConfiguration : IEntityTypeConfiguration<Author>
{
  public void Configure(EntityTypeBuilder<Author> builder)
  {
    builder.ToTable("Authors");
    
    builder.Property(a => a.FirstName).HasColumnType("nvarchar(255)").IsRequired();
    builder.Property(a => a.MiddleName).HasColumnType("nvarchar(255)");
    builder.Property(a => a.LastName).HasColumnType("nvarchar(255)");
  }
}
