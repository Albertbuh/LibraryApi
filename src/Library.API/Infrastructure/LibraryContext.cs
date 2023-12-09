using Library.API.Infrastructure.EntityConfigurations;
using Library.API.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Infrastructure;

public class LibraryContext : DbContext
{
  public DbSet<Genre> Genres { get; set; } = null!;
  public DbSet<Author> Authors { get; set; } = null!;
  public DbSet<BookEdition> BookEditions { get; set; } = null!;
  public DbSet<BookInstance> BookInstances { get; set; } = null!;

  public LibraryContext()
    : base() { }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    var config = new ConfigurationBuilder()
      .AddJsonFile("appsettings.json")
      .SetBasePath(Directory.GetCurrentDirectory())
      .Build();
    
    optionsBuilder.UseMySql(
        config.GetConnectionString("DefaultConnection"),
        Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql")
        );
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    try
    {
      modelBuilder.ApplyConfiguration(new AuthorTypeConfiguration());
      modelBuilder.ApplyConfiguration(new GenreTypeConfiguration());
      modelBuilder.ApplyConfiguration(new BookEditionTypeConfiguration());
      modelBuilder.ApplyConfiguration(new BookInstanceTypeConfiguration());
    }
    catch (Exception e)
    {
      throw new LibraryContextException(e.ToString());
    }
  }
}
