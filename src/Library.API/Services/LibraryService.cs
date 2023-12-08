using Library.API.Services.Exceptions;
using Library.API.DAO;
using Library.API.DAO.Exceptions;

namespace Library.API.Services;

public class LibraryService : ILibraryService 
{
  private ILibraryDAO dao;
  public LibraryService()
  {
    dao = DAOFactory.Create().CreateLibraryDAO();
  }

  public IList<BookInstance> GetAllBooks()
  {
    IList<BookInstance>? result = null;
    try 
    {
      result = dao.GetAllBooks(); 
    }
    catch(LibraryDAOException e)
    {
      throw new LibraryServiceException("Error in getting list of book instances", e);
    }
    return result;
  }

  public async Task<BookInstance?> GetBookInstanceById(int id)
  {
    if(id <= 0)
      return null;
    
    BookInstance? result;
    
    try
    {
      result = await dao.GetBookInstanceById(id);
    }
    catch (LibraryDAOException e)
    {
      throw new LibraryServiceException($"Error in getting book by id -> {id}", e);
    }

    return result;
  }

  public async Task<BookEdition?> GetBookByISBN(string isbn)
  {
    if(!IsISBNValid(isbn))
      return null;
    
    BookEdition? result;
    
    try
    {
      result = await dao.GetBookByISBN(isbn);
    }
    catch (LibraryDAOException e)
    {
      throw new LibraryServiceException($"Error in getting book by id -> {isbn}", e);
    }
    
    return result;
  }

  public async Task<LibraryServiceResponse> AddBookEdition(BookEdition bookInfo)
  {
    if(!IsISBNValid(bookInfo.ISBN))
      return new LibraryServiceResponse("Invalid ISBN", false);
    
    try
    {
      await dao.AddBookEdition(bookInfo);
    }
    catch (LibraryDAOException e)
    {
      try 
      {
        var bookEdition = GetBookByISBN(bookInfo.ISBN);
        if(bookEdition != null)
          throw new LibraryServiceException($"Book with ISBN -> {bookInfo.ISBN} is already in DB", e);
      }
      finally
      {
        throw new LibraryServiceException($"Error while add new edition");
      }
    }
    return new LibraryServiceResponse($"BookEdition with ISBN {bookInfo.ISBN} added to database", true);
  }

  public async Task<LibraryServiceResponse> AddBookInstances(string isbn, int amount)
  {
    if(!IsISBNValid(isbn))
      return new LibraryServiceResponse("Invalid ISBN", false);

    
    try
    {
      await dao.AddBookInstances(isbn, amount);
    }
    catch (LibraryDAOException e)
    {
      throw new LibraryServiceException($"Error while add new instances", e);
    }
    
    return new LibraryServiceResponse($"Added new book instances for {isbn} in {amount} copies", true);
      
  }

  public async Task<LibraryServiceResponse> DeleteBookEdition(string isbn)
  {
    if(!IsISBNValid(isbn))
      return new LibraryServiceResponse("Invalid ISBN", false);
    
    try
    {
      await dao.DeleteBookEdition(isbn);
    }
    catch (LibraryDAOException e)
    {
      throw new LibraryServiceException($"Error while delete edition: {isbn}", e);
    }
    return new LibraryServiceResponse($"Successfull deletion of book edition {isbn}", true);
  }

  public async Task<LibraryServiceResponse> DeleteBookInstance(int id)
  {
    if(id <= 0)
      return new LibraryServiceResponse("Id < 0", false);
    
    try
    {
      await dao.DeleteBookInstance(id);
    }
    catch (LibraryDAOException e)
    {
      throw new LibraryServiceException($"Error while delete edition: {id}", e);
    }
    return new LibraryServiceResponse($"Successfull deletion of book instance with id {id}", true);
  }

  public async Task<LibraryServiceResponse> UpdateBookEdition(string isbn, BookEdition newInfo)
  {
    if(!IsISBNValid(isbn))
      return new LibraryServiceResponse($"Invalid ISBN {isbn}", false);
    if(!IsISBNValid(newInfo.ISBN))
      return new LibraryServiceResponse($"Invalid ISBN of BookEdition update info {newInfo.ISBN}", false);
    
    try
    {
      await dao.UpdateBookEdition(isbn, newInfo);
    }
    catch (LibraryDAOException e)
    {
      throw new LibraryServiceException($"Error in book updating", e);
    }
    return new LibraryServiceResponse($"Book Edition has been updated", true);
  }

  public async Task<LibraryServiceResponse> UpdateBookInstance(int id, BookInstance newInfo)
  {
    if(id <= 0)
      return new LibraryServiceResponse("Id < 0", false);
    if(!IsISBNValid(newInfo.Book.ISBN))
      return new LibraryServiceResponse($"Invalid ISBN of Book instance update info {newInfo.Book.ISBN}", false);
      
    
    try
    {
      await dao.UpdateBookInstance(id, newInfo);
    }
    catch (LibraryDAOException e)
    {
      throw new LibraryServiceException($"Error in book updating", e);
    }
    return new LibraryServiceResponse($"Invalid ISBN of Book instance update info {newInfo.Book.ISBN}", false);
  }

  private bool IsISBNValid(string isbn)
  {
    const string pattern = @"^(?=(?:\D*\d){10}(?:(?:\D*\d){3})?$)[\d-]+$";
    return System.Text.RegularExpressions.Regex.IsMatch(isbn, pattern);
  }

}
