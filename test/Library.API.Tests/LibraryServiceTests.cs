namespace Library.API.Tests;

public class LibraryServiceTests
{
  private ILibraryService service;

  public LibraryServiceTests()
  {
    service = new LibraryService();
  }
  
  [Fact]
  public void CreateNewBookEdition() 
  {
    BookEdition bookInfo = new BookEdition();
    bookInfo.Title = "hobbit";
    bookInfo.ISBN = "12-ed-32342";
    bookInfo.Description = "Это история о маленьком хоббите";
    bookInfo.Authors = new List<Author>() {
      new Author("Пушкин", "Александр", "Сергеевич"),
      new Author("Достоевский", "Фёдор")
    };
    bookInfo.Genres = new List<Genre>() {
      new Genre("комедия"),
      new Genre("триллер")
    };
    service.AddBookEdition(bookInfo);
  }
}
