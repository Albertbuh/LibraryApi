namespace Library.API.Repositories;

public class RepositoryFactory
{
  private ILibraryRepository libraryRepository = new LibraryRepository();

  public static RepositoryFactory Create()
  {
    return new RepositoryFactory();
  }

  public ILibraryRepository CreateLibraryRepository()
  {
    return libraryRepository;
  }

}
