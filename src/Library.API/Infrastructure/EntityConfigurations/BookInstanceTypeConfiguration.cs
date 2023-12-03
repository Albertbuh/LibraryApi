using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Library.API.Models;
namespace Library.API.Infrastructure.EntityConfigurations;

public class BookInstanceTypeConfiguration : IEntityTypeConfiguration<BookInstance>
{
  public void Configure(EntityTypeBuilder<BookInstance> builder)
  {
    builder.ToTable("book_instances");

    builder.Property(bi => bi.Id).HasColumnName("bi_id");
    builder.Property(bi => bi.BookEditionId).HasColumnName("bi_book_edition_id");
    builder.Property(bi => bi.DateOfTaken).HasColumnName("bi_date_of_taken");
    builder.Property(bi => bi.DateOfReturn).HasColumnName("bi_date_of_return");
  }
}
