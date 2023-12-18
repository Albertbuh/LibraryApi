namespace Library.API.Services;

public interface ILibraryService 
{
  public IList<BookEditionDTO> GetAllBooks();
  public IList<BookInstanceDTO> GetAllBookInstances();
  public Task<BookInstanceDTO?> GetBookInstanceById(int id);
  public Task<BookEditionDTO?> GetBookByISBN(string isbn);
  public Task<LibraryServiceResponse> AddBookEdition(BookEditionDTO bookInfo);
  public Task<LibraryServiceResponse> AddBookInstances(string isbn, int amount);
  public Task<LibraryServiceResponse> UpdateBookInstance(int id, BookInstanceDTO newInfo);
  public Task<LibraryServiceResponse> UpdateBookEdition(string isbn, BookEditionDTO newInfo);
  public Task<LibraryServiceResponse> DeleteBookInstance(int id);
  public Task<LibraryServiceResponse> DeleteBookEdition(string isbn);
}
