namespace Library.API.Infrastructure.Seed;
public interface IDatabaseInitializer
{
  public void Seed(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder);
}
