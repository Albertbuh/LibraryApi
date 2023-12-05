using Library.API.Models;
namespace Library.API.Services;

public interface ILibraryService 
{
  public IList<BookInstance> GetAllBooks();
  public Task<BookInstance?> GetBookInstanceById(int id);
  public Task<BookEdition?> GetBookByISBN(string isbn);
  public Task AddBookEdition(BookEdition bookInfo);
  public Task AddBookInstances(string isbn, int amount);
  public Task UpdateBookInstance(int id, BookInstance newInfo);
  public Task UpdateBookEdition(string isbn, BookEdition newInfo);
  public Task DeleteBookInstance(int id);
  public Task DeleteBookEdition(string isbn);
}
