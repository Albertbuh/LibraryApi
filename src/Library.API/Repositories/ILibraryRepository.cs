namespace Library.API.Repositories;

public interface ILibraryRepository 
{
  public IList<BookEdition> GetAllBooks();
  public IList<BookInstance> GetAllBookInstances();
  public Task<BookInstance?> GetBookInstanceById(int id);
  public Task<BookEdition?> GetBookByISBN(string isbn);
  public Task<bool> AddBookEdition(BookEdition bookInfo);
  public Task<bool> AddBookInstances(BookInstance[] bookInstances);
  public Task<bool> UpdateBookInstance(int id, BookInstance newInfo);
  public Task<bool> UpdateBookEdition(string isbn, BookEdition newInfo);
  public Task<bool> DeleteBookInstance(int id);
  public Task<bool> DeleteBookEdition(string isbn);
}
