using Library.API.DAO;
using Library.API.DAO.Exceptions;
using Library.API.Services.Exceptions;

namespace Library.API.Services;

public class LibraryService : ILibraryService
{
  private ILibraryDAO dao;
  private ILogger logger;

  public LibraryService()
  {
    dao = DAOFactory.Create().CreateLibraryDAO();

    using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
    logger = factory.CreateLogger("LibraryService");
  }

  public IList<BookEdition> GetAllBooks()
  {
    IList<BookEdition>? result = null;
    try
    {
      result = dao.GetAllBooks();
    }
    catch (LibraryDAOException e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryServiceException(e.ToString(), e);
    }
    return result;
  }

  public IList<BookInstance> GetAllBookInstances()
  {
    IList<BookInstance>? result = null;
    try
    {
      result = dao.GetAllBookInstances();
    }
    catch (LibraryDAOException e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryServiceException(e.ToString(), e);
    }
    return result;
  }

  public async Task<BookInstance?> GetBookInstanceById(int id)
  {
    if (id <= 0)
      return null;

    BookInstance? result;

    try
    {
      result = await dao.GetBookInstanceById(id);
    }
    catch (LibraryDAOException e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryServiceException(e.ToString(), e);
    }

    return result;
  }

  public async Task<BookEdition?> GetBookByISBN(string isbn)
  {
    if (!IsISBNValid(isbn))
      return null;

    BookEdition? result;

    try
    {
      result = await dao.GetBookByISBN(isbn);
    }
    catch (LibraryDAOException e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryServiceException(e.ToString(), e);
    }

    return result;
  }

  public async Task<LibraryServiceResponse> AddBookEdition(BookEdition bookInfo)
  {
    if (!IsISBNValid(bookInfo.ISBN))
      return new LibraryServiceResponse("Invalid ISBN", false);

    var response = new LibraryServiceResponse(
      $"BookEdition with ISBN {bookInfo.ISBN} added to database",
      true
    );

    try
    {
      var isAdded = await dao.AddBookEdition(bookInfo);

      if (!isAdded)
      {
        response = new LibraryServiceResponse(
          $"Unable to add book edition isbn -> {bookInfo.ISBN}",
          false
        );
      }
    }
    catch (LibraryDAOException e)
    {
      try
      {
        var bookEdition = GetBookByISBN(bookInfo.ISBN);
        if (bookEdition != null)
          throw new LibraryServiceException(
            $"Book with ISBN -> {bookInfo.ISBN} is already in DB",
            e
          );
      }
      finally
      {
        logger.LogWarning(e.ToString());
        throw new LibraryServiceException(e.ToString(), e);
      }
    }
    return response;
  }

  public async Task<LibraryServiceResponse> AddBookInstances(string isbn, int amount)
  {
    if (!IsISBNValid(isbn))
      return new LibraryServiceResponse("Invalid ISBN", false);

    var result = new LibraryServiceResponse(
      $"Added new book instances for {isbn} in {amount} copies",
      true
    );

    try
    {
      bool isAdded = await dao.AddBookInstances(isbn, amount);

      if (!isAdded)
      {
        result = new LibraryServiceResponse(
          $"Unable to add instances for isbn -> {isbn}, check if it exists",
          false
        );
      }
    }
    catch (LibraryDAOException e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryServiceException(e.ToString(), e);
    }

    return result;
  }

  public async Task<LibraryServiceResponse> DeleteBookEdition(string isbn)
  {
    if (!IsISBNValid(isbn))
      return new LibraryServiceResponse("Invalid ISBN", false);

    try
    {
      bool isDeleted = await dao.DeleteBookEdition(isbn);

      if (!isDeleted)
        return new LibraryServiceResponse($"Not found edition with isbn -> {isbn}", false);
    }
    catch (LibraryDAOException e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryServiceException(e.ToString(), e);
    }

    return new LibraryServiceResponse($"Successfull deletion of book edition {isbn}", true);
  }

  public async Task<LibraryServiceResponse> DeleteBookInstance(int id)
  {
    if (id <= 0)
      return new LibraryServiceResponse("Id < 0", false);

    try
    {
      bool isDeleted = await dao.DeleteBookInstance(id);

      if (!isDeleted)
        return new LibraryServiceResponse($"Instance with id -> {id} not founded", false);
    }
    catch (LibraryDAOException e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryServiceException(e.ToString(), e);
    }
    return new LibraryServiceResponse($"Successfull deletion of book instance with id {id}", true);
  }

  public async Task<LibraryServiceResponse> UpdateBookEdition(string isbn, BookEdition newInfo)
  {
    if (!IsISBNValid(isbn))
      return new LibraryServiceResponse($"Invalid ISBN {isbn}", false);
    if (!IsISBNValid(newInfo.ISBN))
      return new LibraryServiceResponse(
        $"Invalid ISBN of BookEdition update info {newInfo.ISBN}",
        false
      );

    try
    {
      await dao.UpdateBookEdition(isbn, newInfo);
    }
    catch (LibraryDAOException e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryServiceException(e.ToString(), e);
    }
    return new LibraryServiceResponse($"Book Edition has been updated", true);
  }

  public async Task<LibraryServiceResponse> UpdateBookInstance(int id, BookInstance newInfo)
  {
    if (id <= 0)
      return new LibraryServiceResponse("Id < 0", false);
    if (!IsISBNValid(newInfo.Book.ISBN))
      return new LibraryServiceResponse(
        $"Invalid ISBN of Book instance update info: {newInfo.Book.ISBN}",
        false
      );

    LibraryServiceResponse response = new LibraryServiceResponse(
      $"New inforamtion for book instance with id -> {id} successfully loaded",
      true
    );

    try
    {
      var isUpdated = await dao.UpdateBookInstance(id, newInfo);

      if (!isUpdated)
        response = new LibraryServiceResponse(
          $"Unable to update book with id -> {id}, try to check if this book exists",
          false
        );
    }
    catch (LibraryDAOException e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryServiceException(e.ToString(), e);
    }

    return response;
  }

  private bool IsISBNValid(string isbn)
  {
    const string pattern = @"^(?=(?:\D*\d){10}(?:(?:\D*\d){3})?$)[\d-]+$";
    return System.Text.RegularExpressions.Regex.IsMatch(isbn, pattern);
  }
}
