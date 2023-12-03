namespace Library.API.Infrastructure.Seed;

public class DatabaseSeedFactory
{
  private static readonly DatabaseSeedFactory factory = new DatabaseSeedFactory();
  private static readonly IDatabaseInitializer dbInit = new LibraryDbInitializer();

  public static DatabaseSeedFactory Create()
  {
    return factory;
  }

  public IDatabaseInitializer CreateDbInitializer()
  {
    return dbInit;
  }
}
