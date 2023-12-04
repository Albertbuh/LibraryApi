using Xunit.Abstractions;

namespace Library.API.Tests;

public class LibraryServiceTests
{
  private ILibraryService service;
  private ITestOutputHelper output;
  public LibraryServiceTests(ITestOutputHelper helper)
  {
    service = new LibraryService();
    output = helper;
  }

  [Fact]
  public async void GetBookEditionByISBN()
  {
    var bookEdition = await service.GetBookByISBN("978-5-04-111308-7");
    Assert.NotNull(bookEdition);

    var genreNames = bookEdition.Genres.Select(g => g.Name);
    var authorNames = bookEdition.Authors.Select(a => a.FirstName);

    Assert.Equal(bookEdition.Title, "Герой нашего времени");
    Assert.Contains("драма", genreNames);
    Assert.Contains("Лермонтов", authorNames);
  }

  [Fact]
  public void GetAllBooksAndCheckIfAllEntitiesAreLoaded()
  {
    var bookList = service.GetAllBooks();

    var testAuthor = new Author("Лермонтов", "Михаил", "Юрьевич");
    var testGenre = new Genre("драма");

    if (bookList.Any())
    {
      Assert.NotNull(bookList.FirstOrDefault(b => b.Book.Authors.Contains(testAuthor)));
      Assert.NotNull(bookList.FirstOrDefault(b => b.Book.Genres.Contains(testGenre)));
    }
    else
    {
      Assert.True(false);
      output.WriteLine("BookInstance entity is empty");
    }
  }

  [Fact(Skip = "one-time test to verify that addition of book instances to db is correct")]
  public void AddBookInstances()
  {
    service.AddBookInstances("978-5-04-111308-7", 3);
  }

  [Fact(Skip = "no need at the moment")]
  public async void GetBookInstanceWithMinimalIdAndTryToDeleteIt()
  {
    BookInstance? bookInstance;
    int minId = 0;
    int serviceRequestCount = 0;
    do
    {
      minId++;
      bookInstance = await service.GetBookInstanceById(minId);
      serviceRequestCount++;
    } while (bookInstance == null && serviceRequestCount < 20);

    service.DeleteBookInstance(minId);
    Assert.NotNull(bookInstance);
  }

  [Fact(Skip = "no need at the moment")]
  public void CreateNewBookEdition()
  {
    BookEdition bookInfo = new BookEdition();
    bookInfo.Title = "TestBook";
    bookInfo.ISBN = "987-1-234-56789-0";
    bookInfo.Description = "Some description with кириллица";
    bookInfo.Authors = new List<Author>()
    {
      new Author("Пушкин", "Александр", "Сергеевич"),
      new Author("Достоевский", "Фёдор")
    };
    bookInfo.Genres = new List<Genre>() { new Genre("комедия"), new Genre("триллер") };
    service.AddBookEdition(bookInfo);
  }
}
