using Library.API.Models;
using Library.API.Infrastructure.EntityConfigurations;
using Library.API.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Library.API.Infrastructure.Seed;

namespace Library.API.Infrastructure;

public class LibraryContext : DbContext
{
  private readonly StreamWriter logStream = new StreamWriter(
    $"{Directory.GetCurrentDirectory()}/Infrastructure/Logs/LibraryContextLogs.txt"
  );
  
  public DbSet<BookEdition> BookEditions { get; set; } = null!;
  public DbSet<BookInstance> BookInstances { get; set; } = null!;

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
      optionsBuilder.LogTo(logStream.WriteLine);
    }
    catch (Exception e)
    {
      throw new LibraryContextException("Error while connecting to database server", e);
    }
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder) 
  { 
    modelBuilder.ApplyConfiguration(new BookTypeConfiguration());
    modelBuilder.ApplyConfiguration(new TakenBookTypeConfiguration());
    modelBuilder.ApplyConfiguration(new AuthorTypeConfiguration());
    modelBuilder.ApplyConfiguration(new GenreTypeConfiguration());

    DatabaseSeedFactory.Create().CreateDbInitializer().Seed(modelBuilder);
  }

  public override void Dispose()
  {
    base.Dispose();
    logStream.Dispose();
  }

  public override async ValueTask DisposeAsync()
  {
    await base.DisposeAsync();
    await logStream.DisposeAsync();
  }
}
