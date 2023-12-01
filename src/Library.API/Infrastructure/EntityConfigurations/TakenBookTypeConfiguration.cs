using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Library.API.Models;
namespace Library.API.Infrastructure.EntityConfigurations;

public class TakenBookTypeConfiguration : IEntityTypeConfiguration<TakenBook>
{
  public void Configure(EntityTypeBuilder<TakenBook> builder)
  {
    builder.ToTable("TakenBooks");
  }
}
