using Library.API.Models;
using Library.API.Infrastructure.EntityConfigurations;
using Library.API.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Library.API.Infrastructure.Seed;

namespace Library.API.Infrastructure;

public class LibraryContext : DbContext
{
  public DbSet<BookEdition> BookEditions { get; set; } = null!;
  public DbSet<BookInstance> BookInstances { get; set; } = null!;
  public DbSet<Genre> Genres {get; set;} = null!;
  public DbSet<Author> Authors {get; set;} = null!;

  public LibraryContext()
  {
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    try
    {
      optionsBuilder.UseMySql(
        "server=localhost;user=root;password=root;database=library",
        Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql")
      );
      // optionsBuilder.LogTo(logStream.WriteLine);
    }
    catch (Exception e)
    {
      throw new LibraryContextException("Error while connecting to database server", e);
    }
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder) 
  { 
    modelBuilder.ApplyConfiguration(new BookEditionTypeConfiguration());
    modelBuilder.ApplyConfiguration(new BookInstanceTypeConfiguration());
    modelBuilder.ApplyConfiguration(new AuthorTypeConfiguration());
    modelBuilder.ApplyConfiguration(new GenreTypeConfiguration());
   
    DatabaseSeedFactory.Create().CreateDbInitializer().Seed(modelBuilder);
  }
}
