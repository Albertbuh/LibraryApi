using Library.API.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Repositories;

public class LibraryRepository : ILibraryRepository
{
  private LibraryContext context = new LibraryContext();
  private ILogger logger;

  public LibraryRepository()
  {
    using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
    logger = factory.CreateLogger("LibraryRepository");
  }

  public IList<BookEdition> GetAllBooks()
  {
    return context.BookEditions.Include(b => b.Authors).Include(b => b.Genres).ToList();
  }

  public IList<BookInstance> GetAllBookInstances()
  {
    return context
      .BookInstances
      .Include(bi => bi.Book.Genres)
      .Include(bi => bi.Book.Authors)
      .ToList();
  }

  public async Task<BookInstance?> GetBookInstanceById(int id)
  {
    return await context
      .BookInstances
      .Include(bi => bi.Book.Genres)
      .Include(bi => bi.Book.Authors)
      .SingleOrDefaultAsync(b => b.Id == id);
  }

  public async Task<BookEdition?> GetBookByISBN(string isbn)
  {
    return await context
      .BookEditions
      .Include(be => be.Genres)
      .Include(be => be.Authors)
      .SingleOrDefaultAsync(be => be.ISBN == isbn);
  }

  public async Task<bool> AddBookEdition(BookEdition bookInfo)
  {
    // bookInfo.Genres = FilterCorrectGenres(bookInfo.Genres);
    // bookInfo.Authors = FilterCorrectAuthors(bookInfo.Authors);

    // Get links to neccessary authors and genres by their names
    bookInfo.Genres = (List<Genre>)context.Genres.Where(g => bookInfo.Genres.Contains(g));
    bookInfo.Authors = (List<Author>)context.Authors.Where(a => bookInfo.Authors.Contains(a));

    context.BookEditions.Add(bookInfo);
    var affectedRowsNumber = await context.SaveChangesAsync();

    return affectedRowsNumber != 0;
  }

  public async Task<bool> AddBookInstances(string isbn, int amount)
  {
    int affectedRowsNumber = 0;
    var edition = context.BookEditions.SingleOrDefault(be => be.ISBN.Equals(isbn));

    if (edition != null)
    {
      var instancesList = new List<BookInstance>();
      for (int i = 0; i < amount; i++)
        instancesList.Add(new BookInstance(edition));

      await context.BookInstances.AddRangeAsync(instancesList);
      affectedRowsNumber = await context.SaveChangesAsync();
    }
    
    return affectedRowsNumber != 0;
  }

  public async Task<bool> DeleteBookEdition(string isbn)
  {
    bool result = false;
      var edition = context.BookEditions.SingleOrDefault(be => be.ISBN == isbn);
      if (edition != null)
      {
        context.BookEditions.Remove(edition);
        await context.SaveChangesAsync();
        result = true;
      }
    return result;
  }

  public async Task<bool> DeleteBookInstance(int id)
  {
    bool result = false;
      var instance = context.BookInstances.SingleOrDefault(be => be.Id == id);
      if (instance != null)
      {
        context.BookInstances.Remove(instance);
        await context.SaveChangesAsync();
        result = true;
      }
    return result;
  }

  public async Task<bool> UpdateBookEdition(string isbn, BookEdition newInfo)
  {
    bool result = false;
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

    return result;
  }

  public async Task<bool> UpdateBookInstance(int id, BookInstance newInfo)
  {
    bool result = false;
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
