namespace Library.API.Repositories.Exceptions;

public class LibraryRepositoryException : Exception
{
  public LibraryRepositoryException() { }

  public LibraryRepositoryException(string message)
    : base(message) { }

  public LibraryRepositoryException(string message, Exception e)
    : base(message, e) { }
}
