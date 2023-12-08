using Library.API.Infrastructure;
using Library.API.Infrastructure.Exceptions;
using Library.API.DAO.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Library.API.DAO;

public class LibraryDAO : ILibraryDAO 
{
  private LibraryContext context = new LibraryContext();

  public IList<BookInstance> GetAllBooks()
  {
    IList<BookInstance> bookList;

    try
    {
      bookList = context
        .BookInstances
        .Include(bi => bi.Book.Genres)
        .Include(bi => bi.Book.Authors)
        .ToList();
    }
    catch (LibraryContextException e)
    {
      throw new LibraryDAOException("Error while getting all books", e);
    }
    catch (Exception e)
    {
      throw new LibraryDAOException("Error while getting all books", e);
    }

    return bookList;
  }

  public async Task<BookInstance?> GetBookInstanceById(int id)
  {
    BookInstance? result;

    try
    {
      result = await context
        .BookInstances
        .Include(bi => bi.Book.Genres)
        .Include(bi => bi.Book.Authors)
        .SingleOrDefaultAsync(b => b.Id == id);
    }
    catch (LibraryContextException e)
    {
      throw new LibraryDAOException($"Error in getting book by id -> {id}", e);
    }
    catch (Exception e)
    {
      throw new LibraryDAOException($"Error in getting book by id -> {id}", e);
    }

    return result;
  }

  public async Task<BookEdition?> GetBookByISBN(string isbn)
  {
    BookEdition? result;

    try
    {
      result = await context
        .BookEditions
        .Include(be => be.Genres)
        .Include(be => be.Authors)
        .SingleOrDefaultAsync(be => be.ISBN == isbn);
    }
    catch (LibraryContextException e)
    {
      throw new LibraryDAOException($"Error in getting book by ISBN -> {isbn}", e);
    }
    catch (Exception e)
    {
      throw new LibraryDAOException($"Error in getting book by ISBN -> {isbn}", e);
    }

    return result;
  }

  public async Task<bool> AddBookEdition(BookEdition bookInfo)
  {
    bool result = false;
    try
    {
      bookInfo.Genres = FilterCorrectGenres(bookInfo.Genres);
      bookInfo.Authors = FilterCorrectAuthors(bookInfo.Authors);
      
      context.BookEditions.Add(bookInfo);
      await context.SaveChangesAsync();
      result = true;
    }
    catch (LibraryContextException e)
    {
      throw new LibraryDAOException($"Error while add new edition", e);
    }
    catch (Exception e)
    {
      throw new LibraryDAOException($"Error while add new edition", e);
    }

    return result;
  }

  public async Task<bool> AddBookInstances(string isbn, int amount)
  {
    bool result = false;
    try
    {
      var edition = context.BookEditions.Single(be => be.ISBN.Equals(isbn));
      var instancesList = new List<BookInstance>();
      for (int i = 0; i < amount; i++)
        instancesList.Add(new BookInstance(edition));

      await context.BookInstances.AddRangeAsync(instancesList);
      await context.SaveChangesAsync();
      result = true;
    }
    catch (LibraryContextException e)
    {
      throw new LibraryDAOException($"Error while add new instances", e);
    }
    catch (Exception e)
    {
      throw new LibraryDAOException($"Error while add new instances", e);
    }
    return result;
  }

  public async Task<bool> DeleteBookEdition(string isbn)
  {
    bool result = false;
    try
    {
      var edition = context.BookEditions.SingleOrDefault(be => be.ISBN == isbn);
      if (edition != null)
      {
        context.BookEditions.Remove(edition);
        await context.SaveChangesAsync();
        result = true;
      }
    }
    catch (LibraryContextException e)
    {
      throw new LibraryDAOException($"Error while delete edition: {isbn}", e);
    }
    catch (Exception e)
    {
      throw new LibraryDAOException($"Error while delete edition: {isbn}", e);
    }
    return result;
  }

  public async Task<bool> DeleteBookInstance(int id)
  {
    bool result = false;
    try
    {
      var instance = context.BookInstances.SingleOrDefault(be => be.Id == id);
      if (instance != null)
      {
        context.BookInstances.Remove(instance);
        await context.SaveChangesAsync();
        result = true;
      }
    }
    catch (LibraryContextException e)
    {
      throw new LibraryDAOException($"Error while delete edition: {id}", e);
    }
    catch (Exception e)
    {
      throw new LibraryDAOException($"Error while delete edition: {id}", e);
    }
    return result;
  }

  public async Task<bool> UpdateBookEdition(string isbn, BookEdition newInfo)
  {
    bool result = false;
    try
    {
      var edition = context.BookEditions.SingleOrDefault(be => be.ISBN.Equals(isbn));
      if (edition != null)
      {
        edition.ISBN = newInfo.ISBN;
        edition.Genres = newInfo.Genres;
        edition.Authors = newInfo.Authors;
        edition.Description = newInfo.Description;
        edition.Title = newInfo.Title;

        await context.SaveChangesAsync();
        result = true;
      }
    }
    catch (LibraryContextException e)
    {
      throw new LibraryDAOException($"Error in book updating", e);
    }
    catch (Exception e)
    {
      throw new LibraryDAOException($"Error in book updating", e);
    }
    
    return result;
  }

  public async Task<bool> UpdateBookInstance(int id, BookInstance newInfo)
  {
    bool result = false;
    try
    {
      var instance = await context.BookInstances.SingleOrDefaultAsync(bi => bi.Id == id);
      if (instance != null)
      {
        instance.DateOfTaken = newInfo.DateOfTaken;
        instance.DateOfReturn = newInfo.DateOfReturn;
        await context.SaveChangesAsync();
        result = true;
      }
    }
    catch (LibraryContextException e)
    {
      throw new LibraryDAOException($"Error in book updating", e);
    }
    catch (Exception e)
    {
      throw new LibraryDAOException($"Error in book updating", e);
    }

    return result;
  }

  private List<Genre> FilterCorrectGenres(IList<Genre> genres)
  {
    List<Genre> newGenres = new();
    foreach(var item in context.Genres)
    {
      if(genres.Contains(item))
        newGenres.Add(item);
    }
    return newGenres;
  }

  private List<Author> FilterCorrectAuthors(IList<Author> authors)
  {
    List<Author> newAuthors = new();
    foreach (var item in context.Authors)
    {
      if(authors.Contains(item))
        newAuthors.Add(item);
    }
    return newAuthors;
  }

}
