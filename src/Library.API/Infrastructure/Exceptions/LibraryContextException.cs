namespace Library.API.Infrastructure.Exceptions;

public class LibraryContextException : Exception
{
  public LibraryContextException() { }

  public LibraryContextException(string message)
    : base(message) { }

  public LibraryContextException(string message, Exception e)
    : base(message, e) { }
}
