namespace Library.API.Services.Exceptions;

public class LibraryServiceException : Exception
{
  public LibraryServiceException() { }

  public LibraryServiceException(string message)
    : base(message) { }

  public LibraryServiceException(string message, Exception e)
    : base(message, e) { }
}
