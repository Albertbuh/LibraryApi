using Library.API.Models;
namespace Library.API.Services;

public interface ILibraryService 
{
  public IList<BookInstance> GetAllBooks();
  public BookInstance GetBookById(int id);
  public BookEdition GetBookByISBN(string isbn);
  public void AddBookEdition(BookEdition bookInfo);
  public void AddBookInstances(string isbn, int amount);
  public void UpdateBookInstance(int id, BookInstance newInfo);
  public void UpdateBookEdition(string isbn, BookEdition newInfo);
  public void DeleteBookInstance(int id);
  public void DeleteBookEdition(string isbn);
}
