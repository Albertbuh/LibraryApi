using Library.API.Infrastructure;
using Library.API.Infrastructure.Exceptions;
using Library.API.Models;
using Library.API.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Services;

public class LibraryService : ILibraryService
{
  private LibraryContext context = new LibraryContext();

  public LibraryService()
  {
    // var seeder = new LibraryContextSeed();
    // seeder.SeedAsync(context);
  }

  public IList<BookInstance> GetAllBooks()
  {
    IList<BookInstance> bookList;

    try
    {
      bookList = context.BookInstances
                        .Include(bi => bi.Book.Genres)
                        .Include(bi => bi.Book.Authors)
                        .ToList();
    }
    catch (LibraryContextException e)
    {
      throw new LibraryServiceException("Error while getting all books", e);
    }
    catch (Exception e)
    {
      throw new LibraryServiceException("Error while getting all books", e);
    }

    return bookList;
  }

  public async Task<BookInstance?> GetBookInstanceById(int id)
  {
    BookInstance? result;

    try
    {
      result = await context.BookInstances.Include(bi => bi.Book).SingleOrDefaultAsync(b => b.Id == id);
    }
    catch (LibraryContextException e)
    {
      throw new LibraryServiceException($"Error in getting book by id -> {id}", e);
    }
    catch (Exception e)
    {
      throw new LibraryServiceException($"Error in getting book by id -> {id}", e);
    }

    return result;
  }

  public async Task<BookEdition?> GetBookByISBN(string isbn)
  {
    BookEdition? result;

    try
    {
      result = await context.BookEditions
                      .Include(be => be.Genres)
                      .Include(be => be.Authors)
                      .SingleOrDefaultAsync(be => be.ISBN == isbn);
    }
    catch (LibraryContextException e)
    {
      throw new LibraryServiceException($"Error in getting book by ISBN -> {isbn}", e);
    }
    catch (Exception e)
    {
      throw new LibraryServiceException($"Error in getting book by ISBN -> {isbn}", e);
    }

    return result;
  }

  public async void AddBookEdition(BookEdition bookInfo)
  {
    try
    {
      var genres = await context.Genres.Where(g => bookInfo.Genres.Contains(g)).ToListAsync();
      bookInfo.Genres = genres;
      var authors = await context.Authors.Where(a => bookInfo.Authors.Contains(a)).ToListAsync();
      bookInfo.Authors = authors;
      await context.BookEditions.AddAsync(bookInfo);
      await context.SaveChangesAsync();
    }
    catch (LibraryContextException e)
    {
      throw new LibraryServiceException($"Error while add new edition", e);
    }
    catch (Exception e)
    {
      throw new LibraryServiceException($"Error while add new edition", e);
    }
  }

  public async void AddBookInstances(string isbn, int amount)
  {
    try
    {
      var edition = context.BookEditions.Single(be => be.ISBN.Equals(isbn));
      var instancesList = new List<BookInstance>();
      for (int i = 0; i < amount; i++)
        instancesList.Add(new BookInstance(edition));

      await context.BookInstances.AddRangeAsync(instancesList);
      await context.SaveChangesAsync();
    }
    catch (LibraryContextException e)
    {
      throw new LibraryServiceException($"Error while add new instances", e);
    }
    catch (Exception e)
    {
      throw new LibraryServiceException($"Error while add new instances", e);
    }
  }

  public async void DeleteBookEdition(string isbn)
  {
    try
    {
      var edition = context.BookEditions.SingleOrDefault(be => be.ISBN == isbn);
      if (edition != null)
      {
        context.BookEditions.Remove(edition);
        await context.SaveChangesAsync();
      }
    }
    catch (LibraryContextException e)
    {
      throw new LibraryServiceException($"Error while delete edition: {isbn}", e);
    }
    catch (Exception e)
    {
      throw new LibraryServiceException($"Error while delete edition: {isbn}", e);
    }
  }

  public async void DeleteBookInstance(int id)
  {
    try
    {
      var instance = context.BookInstances.SingleOrDefault(be => be.Id == id);
      if (instance != null)
      {
        context.BookInstances.Remove(instance);
        await context.SaveChangesAsync();
      }
    }
    catch (LibraryContextException e)
    {
      throw new LibraryServiceException($"Error while delete edition: {id}", e);
    }
    catch (Exception e)
    {
      throw new LibraryServiceException($"Error while delete edition: {id}", e);
    }
  }

  public async void UpdateBookEdition(string isbn, BookEdition newInfo)
  {
    try
    {
      var edition = context.BookEditions.SingleOrDefault(be => be.ISBN.Equals(isbn));
      if (edition != null)
      {
        // var entry = context.Entry(edition);
        // entry.CurrentValues.SetValues(newInfo);
        edition.ISBN = newInfo.ISBN;
        edition.Genres = newInfo.Genres;
        edition.Authors = newInfo.Authors;
        edition.Description = newInfo.Description;
        edition.Title = newInfo.Title;

        await context.SaveChangesAsync();
      }
    }
    catch (LibraryContextException e)
    {
      throw new LibraryServiceException($"Error in book updating", e);
    }
    catch (Exception e)
    {
      throw new LibraryServiceException($"Error in book updating", e);
    }
  }

  public async void UpdateBookInstance(int id, BookInstance newInfo)
  {
    try
    {
      var instance = await context.BookInstances.SingleOrDefaultAsync(bi => bi.Id == id);
      if(instance != null)
      {
        var entry = context.Entry(instance);
        entry.CurrentValues.SetValues(instance);
      }
    }
    catch (LibraryContextException e)
    {
      throw new LibraryServiceException($"Error in book updating", e);
    }
    catch (Exception e)
    {
      throw new LibraryServiceException($"Error in book updating", e);
    }
  }
}
