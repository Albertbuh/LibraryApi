using Library.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.API.Infrastructure.EntityConfigurations;

public class GenreTypeConfiguration : IEntityTypeConfiguration<Genre>
{
  public void Configure(EntityTypeBuilder<Genre> builder)
  {
    builder.ToTable("Genres");

    builder
      .Property(g => g.Name)
      .HasColumnType("varchar(200)")
      .HasCharSet("utf8mb4")
      .UseCollation("utf8mb4_general_ci")
      .IsRequired();
  }
}
