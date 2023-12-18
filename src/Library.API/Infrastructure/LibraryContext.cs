using Library.API.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Infrastructure;

public class LibraryContext : DbContext
{
    public DbSet<Genre> Genres { get; set; } = null!;
    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<BookEdition> BookEditions { get; set; } = null!;
    public DbSet<BookInstance> BookInstances { get; set; } = null!;

    public LibraryContext() { }

    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AuthorTypeConfiguration());
        modelBuilder.ApplyConfiguration(new GenreTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BookEditionTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BookInstanceTypeConfiguration());
    }
}
