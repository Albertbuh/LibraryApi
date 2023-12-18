using Library.API.Infrastructure;
namespace Library.API.Repositories;

public class RepositoryFactory
{

  public static RepositoryFactory Create()
  {
    return new RepositoryFactory();
  }

  public ILibraryRepository CreateLibraryRepository(LibraryContext context)
  {
    return new LibraryRepository(context);
  }

}
