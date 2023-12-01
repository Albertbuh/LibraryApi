using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Library.API.Models;
namespace Library.API.Infrastructure.EntityConfigurations;

public class BookTypeConfiguration : IEntityTypeConfiguration<Book>
{
  public void Configure(EntityTypeBuilder<Book> builder)
  {
    builder.ToTable("Books");
    
    builder.Property(b => b.ISBN).HasColumnType("varchar(20)").IsRequired();
  }
}
