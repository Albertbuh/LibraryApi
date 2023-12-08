namespace Library.API.DAO;

public interface ILibraryDAO 
{
  public IList<BookInstance> GetAllBooks();
  public Task<BookInstance?> GetBookInstanceById(int id);
  public Task<BookEdition?> GetBookByISBN(string isbn);
  public Task<bool> AddBookEdition(BookEdition bookInfo);
  public Task<bool> AddBookInstances(string isbn, int amount);
  public Task<bool> UpdateBookInstance(int id, BookInstance newInfo);
  public Task<bool> UpdateBookEdition(string isbn, BookEdition newInfo);
  public Task<bool> DeleteBookInstance(int id);
  public Task<bool> DeleteBookEdition(string isbn);
}
