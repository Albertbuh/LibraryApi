using Xunit.Abstractions;

namespace Library.API.Tests;

public class LibraryServiceTests
{
  private ILibraryService service;
  private ITestOutputHelper output;

  private readonly BookEdition testBookEdition;

  public LibraryServiceTests(ITestOutputHelper helper)
  {
    service = new LibraryService();
    output = helper;

    testBookEdition = new BookEdition(
      "987-1-234-56789-0",
      "bookTitle",
      "Some description with кириллица"
    );
    testBookEdition.Authors = new List<Author>()
    {
      new Author("Пушкин", "Александр", "Сергеевич"),
      new Author("Достоевский", "Фёдор")
    };
    testBookEdition.Genres = new List<Genre>() { new Genre("комедия"), new Genre("триллер") };
  }


  [Fact]
  public async void CreateUpdateAndDeleteTestBookEdition() 
  { 
    const string newDesc = "описание на кириллице";
    
    var bookEdition = await service.GetBookByISBN(testBookEdition.ISBN);
    if(bookEdition == null)
    {
      await service.AddBookEdition(testBookEdition);
      bookEdition = await service.GetBookByISBN(testBookEdition.ISBN);
    }
    
    Assert.NotNull(bookEdition);

    bookEdition.Description = newDesc;

    await service.UpdateBookEdition(testBookEdition.ISBN, bookEdition);
    bookEdition = await service.GetBookByISBN(testBookEdition.ISBN);

    Assert.NotNull(bookEdition);
    Assert.Equal(bookEdition.Description, newDesc);

    await service.DeleteBookEdition(testBookEdition.ISBN);
    bookEdition = await service.GetBookByISBN(testBookEdition.ISBN);

    Assert.Null(bookEdition);
  }

  [Fact]
  public async void GetBookEditionByISBNAndCheckIncludeEntities()
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

  [Fact]
  public async void UpdateBookInstanceWithMinimumIdByChangingTakenDate()
  {
    int curId = 0;
    BookInstance? bookInstance;
    do
    {
      curId++;
      bookInstance = await service.GetBookInstanceById(curId);
    } while (bookInstance == null && curId < 30);

    Assert.NotNull(bookInstance);

    BookInstance newInfo = new BookInstance(bookInstance.Book);
    newInfo.DateOfTaken = DateOnly.FromDateTime(DateTime.Now);
    newInfo.DateOfReturn = newInfo.DateOfTaken.Value.AddDays(7);
    await service.UpdateBookInstance(curId, newInfo);

    var newBookInstanceFromContext = await service.GetBookInstanceById(curId);

    Assert.NotNull(newBookInstanceFromContext);
    Assert.NotNull(newBookInstanceFromContext.DateOfTaken);
    Assert.NotNull(newBookInstanceFromContext.DateOfReturn);
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

    await service.DeleteBookInstance(minId);
    Assert.NotNull(bookInstance);
  }
}
