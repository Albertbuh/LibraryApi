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

  public async Task AddBookEdition(BookEdition bookInfo)
  {
    if(!IsISBNValid(bookInfo.ISBN) || bookInfo.Id <= 0)
      return;
    
    try
    {
      await dao.AddBookEdition(bookInfo);
    }
    catch (LibraryDAOException e)
    {
      throw new LibraryDAOException($"Error while add new edition", e);
    }
  }

  public async Task AddBookInstances(string isbn, int amount)
  {
    if(!IsISBNValid(isbn) || amount <= 0)
      return;
    
    try
    {
      await dao.AddBookInstances(isbn, amount);
    }
    catch (LibraryDAOException e)
    {
      throw new LibraryDAOException($"Error while add new instances", e);
    }
  }

  public async Task DeleteBookEdition(string isbn)
  {
    if(!IsISBNValid(isbn))
      return;
    
    try
    {
      await dao.DeleteBookEdition(isbn);
    }
    catch (LibraryDAOException e)
    {
      throw new LibraryDAOException($"Error while delete edition: {isbn}", e);
    }
  }

  public async Task DeleteBookInstance(int id)
  {
    if(id <= 0)
      return;
    
    try
    {
      await dao.DeleteBookInstance(id);
    }
    catch (LibraryDAOException e)
    {
      throw new LibraryDAOException($"Error while delete edition: {id}", e);
    }
  }

  public async Task UpdateBookEdition(string isbn, BookEdition newInfo)
  {
    if(!IsISBNValid(isbn) || !IsISBNValid(newInfo.ISBN))
      return;
    
    try
    {
      await dao.UpdateBookEdition(isbn, newInfo);
    }
    catch (LibraryDAOException e)
    {
      throw new LibraryDAOException($"Error in book updating", e);
    }
  }

  public async Task UpdateBookInstance(int id, BookInstance newInfo)
  {
    if(id <= 0 || !IsISBNValid(newInfo.Book.ISBN))
      return;
    
    try
    {
      await dao.UpdateBookInstance(id, newInfo);
    }
    catch (LibraryDAOException e)
    {
      throw new LibraryDAOException($"Error in book updating", e);
    }
  }

  private bool IsISBNValid(string isbn)
  {
    const string pattern = @"^(?=(?:\D*\d){10}(?:(?:\D*\d){3})?$)[\d-]+$";
    return System.Text.RegularExpressions.Regex.IsMatch(isbn, pattern);
  }

}
