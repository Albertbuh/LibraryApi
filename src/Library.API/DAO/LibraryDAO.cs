using Library.API.DAO.Exceptions;
using Library.API.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Library.API.DAO;

public class LibraryDAO : ILibraryDAO
{
  private LibraryContext context = new LibraryContext();
  private ILogger logger;

  public LibraryDAO()
  {
    using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
    logger = factory.CreateLogger("LibraryDAO");
  }
  
  public IList<BookEdition> GetAllBooks()
  {
    IList<BookEdition> bookList;

    try
    {
      bookList = context.BookEditions.Include(b => b.Authors).Include(b => b.Genres).ToList();
    }
    catch (Exception e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryDAOException(e.ToString());
    }
    return bookList;
  }

  public IList<BookInstance> GetAllBookInstances()
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
    catch (Exception e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryDAOException(e.ToString());
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
    catch (Exception e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryDAOException(e.ToString());
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
    catch (Exception e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryDAOException(e.ToString());
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
    catch (Exception e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryDAOException(e.ToString());
    }

    return result;
  }

  public async Task<bool> AddBookInstances(string isbn, int amount)
  {
    bool result = false;
    try
    {
      var edition = context.BookEditions.SingleOrDefault(be => be.ISBN.Equals(isbn));

      if (edition != null)
      {
        var instancesList = new List<BookInstance>();
        for (int i = 0; i < amount; i++)
          instancesList.Add(new BookInstance(edition));

        await context.BookInstances.AddRangeAsync(instancesList);
        await context.SaveChangesAsync();
        result = true;
      }
    }
    catch (Exception e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryDAOException(e.ToString());
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
    catch (Exception e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryDAOException(e.ToString());
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
    catch (Exception e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryDAOException(e.ToString(), e);
    }
    return result;
  }

  public async Task<bool> UpdateBookEdition(string isbn, BookEdition newInfo)
  {
    bool result = false;
    try
    {
      var edition = context
        .BookEditions
        .Include(be => be.Authors)
        .Include(be => be.Genres)
        .SingleOrDefault(be => be.ISBN.Equals(isbn));

      if (edition != null)
      {
        edition.ISBN = newInfo.ISBN;
        edition.Genres = FilterCorrectGenres(newInfo.Genres);
        edition.Authors = FilterCorrectAuthors(newInfo.Authors);

        edition.Description = newInfo.Description;
        edition.Title = newInfo.Title;

        await context.SaveChangesAsync();
        result = true;
      }
    }
    catch (Exception e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryDAOException(e.ToString(), e);
    }

    return result;
  }

  public async Task<bool> UpdateBookInstance(int id, BookInstance newInfo)
  {
    bool result = false;
    try
    {
      var instance = await context
        .BookInstances
        .Include(bi => bi.Book.Authors)
        .Include(bi => bi.Book.Genres)
        .SingleOrDefaultAsync(bi => bi.Id == id);

      if (instance != null)
      {
        instance.DateOfTaken = newInfo.DateOfTaken;
        instance.DateOfReturn = newInfo.DateOfReturn;
        await context.SaveChangesAsync();
        result = true;
      }
    }
    catch (Exception e)
    {
      logger.LogWarning(e.ToString());
      throw new LibraryDAOException(e.ToString(), e);
    }

    return result;
  }

  private List<Genre> FilterCorrectGenres(IList<Genre> genres)
  {
    List<Genre> newGenres = new();
    foreach (var item in context.Genres)
    {
      if (genres.Contains(item))
        newGenres.Add(item);
    }
    return newGenres;
  }

  private List<Author> FilterCorrectAuthors(IList<Author> authors)
  {
    List<Author> newAuthors = new();
    foreach (var item in context.Authors)
    {
      if (authors.Contains(item))
        newAuthors.Add(item);
    }
    return newAuthors;
  }
}
