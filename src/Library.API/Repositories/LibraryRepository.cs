using Library.API.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Repositories;

public class LibraryRepository : ILibraryRepository
{
  private LibraryContext context;
  private ILogger logger;

  public LibraryRepository(LibraryContext context)
  {
    using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
    logger = factory.CreateLogger("LibraryRepository");

    this.context = context;
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
    bookInfo.Genres = FilterCorrectGenres(bookInfo.Genres);
    bookInfo.Authors = FilterCorrectAuthors(bookInfo.Authors);


    context.BookEditions.Add(bookInfo);
    var affectedRowsNumber = await context.SaveChangesAsync();

    return affectedRowsNumber != 0;
  }

  public async Task<bool> AddBookInstances(BookInstance[] bookInstances)
  {
    int affectedRowsNumber = 0;
    
    await context.BookInstances.AddRangeAsync(bookInstances);
    affectedRowsNumber = await context.SaveChangesAsync();
    
    return affectedRowsNumber != 0;
  }

  public async Task<bool> DeleteBookEdition(string isbn)
  {
    int affectedRowsNumber = 0;

    var edition = context.BookEditions.SingleOrDefault(be => be.ISBN == isbn);
    if (edition != null)
    {
      context.BookEditions.Remove(edition);
      affectedRowsNumber = await context.SaveChangesAsync();
    }

    return affectedRowsNumber != 0;
  }

  public async Task<bool> DeleteBookInstance(int id)
  {
    int affectedRowsNumber = 0;

    var instance = context.BookInstances.SingleOrDefault(be => be.Id == id);
    if (instance != null)
    {
      context.BookInstances.Remove(instance);
      affectedRowsNumber = await context.SaveChangesAsync();
    }

    return affectedRowsNumber != 0;
  }

  public async Task<bool> UpdateBookEdition(string isbn, BookEdition newInfo)
  {
    int affectedRowsNumber = 0;
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

      affectedRowsNumber = await context.SaveChangesAsync();
    }

    return affectedRowsNumber != 0;
  }

  public async Task<bool> UpdateBookInstance(int id, BookInstance newInfo)
  {
    int affectedRowsNumber = 0;
    var instance = await context
      .BookInstances
      .Include(bi => bi.Book.Authors)
      .Include(bi => bi.Book.Genres)
      .SingleOrDefaultAsync(bi => bi.Id == id);

    if (instance != null)
    {
      instance.DateOfTaken = newInfo.DateOfTaken;
      instance.DateOfReturn = newInfo.DateOfReturn;
      affectedRowsNumber = await context.SaveChangesAsync();
    }

    return affectedRowsNumber != 0;
  }

  private List<Genre> FilterCorrectGenres(IList<Genre> genres)
  {
    List<Genre> newGenres = new();
    foreach (var item in context.Genres)
    {
      if (genres.Contains(item))
        newGenres.Add(item);
    }

    var aboba = context.Genres.Where(g => genres.Contains(g));
    foreach(var a in aboba)
    {
      logger.LogInformation($"GENRE {a.ToString().ToUpper()}");
    }
    return newGenres;
    // var result = new List<Genre>();
    // result.AddRange(context.Genres.Where(g => genres.Contains(g)));
    // foreach(var r in result)
    // {
    //   logger.LogInformation(r.ToString().ToUpper());   
    // }
    // return result;
  }

  private List<Author> FilterCorrectAuthors(IList<Author> authors)
  {
    // List<Author> newAuthors = new();
    // foreach (var item in context.Authors)
    // {
    //   if (authors.Contains(item))
    //     newAuthors.Add(item);
    // }
    var result = new List<Author>();
    result.AddRange(context.Authors.Where(a => authors.Contains(a)).Select(g => g));
    return result;
  }
}
