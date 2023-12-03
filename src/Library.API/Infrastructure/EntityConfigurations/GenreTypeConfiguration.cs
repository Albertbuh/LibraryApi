using Library.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.API.Infrastructure.EntityConfigurations;

public class GenreTypeConfiguration : IEntityTypeConfiguration<Genre>
{
  public void Configure(EntityTypeBuilder<Genre> builder)
  {
    builder.ToTable("genres");

    builder.Property(g => g.Id).HasColumnName("g_id");
    builder
      .Property(g => g.Name)
      .HasColumnName("g_name")
      .HasColumnType("nvarchar(200)")
      .IsRequired();
    
    builder
      .HasMany(g => g.BookEditions)
      .WithMany(be => be.Genres)
      .UsingEntity(
        "m2m_editions_genres",
        l => l.HasOne(typeof(BookEdition)).WithMany().HasForeignKey("edition_id"),
        r => r.HasOne(typeof(Genre)).WithMany().HasForeignKey("genre_id"),
        j => j.HasKey("genre_id", "edition_id")
      );
  }
}
