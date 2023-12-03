using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Library.API.Models;
namespace Library.API.Infrastructure.EntityConfigurations;

public class TakenBookTypeConfiguration : IEntityTypeConfiguration<BookInstance>
{
  public void Configure(EntityTypeBuilder<BookInstance> builder)
  {
    builder.ToTable("TakenBooks");
  }
}
