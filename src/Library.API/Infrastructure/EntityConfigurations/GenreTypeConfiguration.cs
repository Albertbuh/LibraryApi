using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Library.API.Models;
namespace Library.API.Infrastructure.EntityConfigurations;

public class GenreTypeConfiguration : IEntityTypeConfiguration<Genre>
{
  public void Configure(EntityTypeBuilder<Genre> builder)
  {
    builder.ToTable("Genres");
    
    builder.Property(g => g.Name).HasColumnType("varchar(100)").IsRequired();
  }
}
