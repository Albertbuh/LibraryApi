namespace Library.API.Services;

public interface ILibraryService 
{
  public IList<BookInstance> GetAllBooks();
  public Task<BookInstance?> GetBookInstanceById(int id);
  public Task<BookEdition?> GetBookByISBN(string isbn);
  public Task<LibraryServiceResponse> AddBookEdition(BookEdition bookInfo);
  public Task<LibraryServiceResponse> AddBookInstances(string isbn, int amount);
  public Task<LibraryServiceResponse> UpdateBookInstance(int id, BookInstance newInfo);
  public Task<LibraryServiceResponse> UpdateBookEdition(string isbn, BookEdition newInfo);
  public Task<LibraryServiceResponse> DeleteBookInstance(int id);
  public Task<LibraryServiceResponse> DeleteBookEdition(string isbn);
}
