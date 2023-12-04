using Library.API.Models;
using Library.API.Infrastructure.EntityConfigurations;
using Library.API.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Infrastructure;

public class LibraryContext : DbContext
{
  public DbSet<Genre> Genres {get; set;} = null!;
  public DbSet<Author> Authors {get; set;} = null!;
  public DbSet<BookEdition> BookEditions { get; set; } = null!;
  public DbSet<BookInstance> BookInstances { get; set; } = null!;
  
  public LibraryContext()
  {
    // Database.EnsureDeleted();
    // Database.EnsureCreated();
  }

  
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    try
    {
      optionsBuilder.UseMySql(
        "server=localhost;user=root;password=root;database=library",
        Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql")
      );
    }
    catch (Exception e)
    {
      throw new LibraryContextException("Error while connecting to database server", e);
    }
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder) 
  { 
    modelBuilder.ApplyConfiguration(new AuthorTypeConfiguration());
    modelBuilder.ApplyConfiguration(new GenreTypeConfiguration());
    modelBuilder.ApplyConfiguration(new BookEditionTypeConfiguration());
    modelBuilder.ApplyConfiguration(new BookInstanceTypeConfiguration());
    
    // DatabaseSeedFactory.Create().CreateDbInitializer().Seed(modelBuilder);
  }
}
