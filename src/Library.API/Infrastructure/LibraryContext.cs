using Library.API.Models;
using Library.API.Infrastructure.EntityConfigurations;
using Library.API.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Infrastructure;

public class LibraryContext : DbContext
{
  private readonly StreamWriter logStream = new StreamWriter(
    $"{Directory.GetCurrentDirectory()}/Infrastructure/Logs/LibraryContextLogs.txt"
  );
  public DbSet<Book> Books { get; set; } = null!;
  public DbSet<TakenBook> TakenBooks { get; set; } = null!;

  public LibraryContext()
  {
    Database.EnsureDeleted();
    Database.EnsureCreated();
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
