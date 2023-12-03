using Library.API.Infrastructure;
using Library.API.Infrastructure.Exceptions;
using Library.API.Models;
using Library.API.Services.Exceptions;
// using Microsoft.EntityFrameworkCore;

namespace Library.API.Services;

public class LibraryService : ILibraryService
{
  private LibraryContext context = new LibraryContext();
  private List<Genre> libraryGenres;
  private List<Author> libraryAuthors;

  public LibraryService()
  {
    libraryGenres = context.Genres.ToList();
    libraryAuthors = context.Authors.ToList();
  }

  public IList<BookInstance> GetAllBooks()
  {
    IList<BookInstance> bookList;
    
    try 
    {
      bookList = context.BookInstances.ToList();
    }
    catch(LibraryContextException e)
    {
      throw new LibraryServiceException("Error while getting all books", e); 
    }
    catch(Exception e)
    {
      throw new LibraryServiceException("Error while getting all books", e); 
    }
    
    return bookList;
  }
 
  public BookInstance GetBookById(int id)
  {
    BookInstance result;
    
    try
    {
      result = context.BookInstances.Single(b => b.Id == id);
    }
    catch(LibraryContextException e)
    {
      throw new LibraryServiceException($"Error in getting book by id -> {id}", e);
    }
    catch(Exception e)
    {
      throw new LibraryServiceException($"Error in getting book by id -> {id}", e);
    }

    return result;
  } 
  

  public BookEdition GetBookByISBN(string isbn)
  {
    BookEdition result;

    try
    {
      result = context.BookEditions.Single(be => be.ISBN == isbn);
    }
    catch(LibraryContextException e)
    {
      throw new LibraryServiceException($"Error in getting book by ISBN -> {isbn}", e);
    }
    catch(Exception e)
    {
      throw new LibraryServiceException($"Error in getting book by ISBN -> {isbn}", e);
    }

    return result;
  }
  
  
  public async void AddBookEdition(BookEdition bookInfo)
  {
    try
    {
      var genres = libraryGenres.Where(g => bookInfo.Genres.Contains(g)).ToList();
      bookInfo.Genres = genres;
      var authors = libraryAuthors.Where(a => bookInfo.Authors.Contains(a)).ToList();
      bookInfo.Authors = authors;
      await context.BookEditions.AddAsync(bookInfo);
      await context.SaveChangesAsync();
    }
    catch(LibraryContextException e)
    {
      throw new LibraryServiceException($"Error while add new edition", e);
    }
    catch(Exception e)
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
      for(int i = 0; i < amount; i++)
        instancesList.Add(new BookInstance(edition));

      await context.BookInstances.AddRangeAsync(instancesList);
      await context.SaveChangesAsync();
    }
    catch(LibraryContextException e)
    {
      throw new LibraryServiceException($"Error while add new instances", e);
    }
    catch(Exception e)
    {
      throw new LibraryServiceException($"Error while add new instances", e);
    }
  }
  
  public async void DeleteBookEdition(string isbn)
  {
    try
    {
      var edition = context.BookEditions.SingleOrDefault(be => be.ISBN == isbn);    
      if(edition != null)
      {
        context.BookEditions.Remove(edition);
        await context.SaveChangesAsync();
      }
    }
    catch(LibraryContextException e)
    {
      throw new LibraryServiceException($"Error while delete edition: {isbn}", e);
    }
    catch(Exception e)
    {
      throw new LibraryServiceException($"Error while delete edition: {isbn}", e);
    }
  }

  public async void DeleteBookInstance(int id)
  {
    try
    {
      var instance = context.BookInstances.SingleOrDefault(be => be.Id == id);    
      if(instance != null)
      {
        context.BookInstances.Remove(instance);
        await context.SaveChangesAsync();
      }
    }
    catch(LibraryContextException e)
    {
      throw new LibraryServiceException($"Error while delete edition: {id}", e);
    }
    catch(Exception e)
    {
      throw new LibraryServiceException($"Error while delete edition: {id}", e);
    }
  }

  public async void UpdateBookEdition(string isbn, BookEdition newInfo)
  {
    try
    {
      var edition = context.BookEditions.SingleOrDefault(be => be.ISBN.Equals(isbn));
      if(edition != null)
      {
        edition.ISBN = newInfo.ISBN;
        edition.Genres = newInfo.Genres;
        edition.Authors = newInfo.Authors;
        edition.Description = newInfo.Description;
        edition.Title = newInfo.Title;
        
        await context.SaveChangesAsync();
      }
    }
    catch(LibraryContextException e)
    {
      throw new LibraryServiceException($"Error in book updating", e);
    }
    catch(Exception e)
    {
      throw new LibraryServiceException($"Error in book updating", e);
    }
  }

  public void UpdateBookInstance(int id, BookInstance newInfo)
  {
    throw new NotImplementedException();
  }
}
