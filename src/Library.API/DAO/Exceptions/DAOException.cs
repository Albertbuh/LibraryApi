namespace Library.API.DAO.Exceptions;

public class LibraryDAOException : Exception
{
  public LibraryDAOException() { }

  public LibraryDAOException(string message)
    : base(message) { }

  public LibraryDAOException(string message, Exception e)
    : base(message, e) { }
}
