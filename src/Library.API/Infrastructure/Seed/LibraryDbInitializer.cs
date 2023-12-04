using Library.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Infrastructure.Seed;

public class LibraryDbInitializer : IDatabaseInitializer
{
  private Dictionary<string, Genre> genreDictionary = new Dictionary<string, Genre>();
  private Dictionary<int, Author> authorDictionary = new Dictionary<int, Author>();
  private List<BookEdition> bookEditions = new List<BookEdition>();

  public LibraryDbInitializer()
  {
    genreDictionary.Add("комедия", new Genre { Name = "комедия", Id = 1 });
    genreDictionary.Add("драма", new Genre { Name = "драма", Id = 2 });
    genreDictionary.Add("трагедия", new Genre { Name = "трагедия", Id = 3 });
    genreDictionary.Add("ужасы", new Genre { Name = "ужасы", Id = 4 });

    authorDictionary.Add(
      1,
      new Author
      {
        FirstName = "Пушкин",
        LastName = "Александр",
        MiddleName = "Сергеевич",
        Id = 1
      }
    );
    authorDictionary.Add(
      2,
      new Author
      {
        FirstName = "Достоевский",
        LastName = "Фёдор",
        MiddleName = "Михайлович",
        Id = 2
      }
    );
    authorDictionary.Add(
      3,
      new Author
      {
        FirstName = "Кинг",
        LastName = "Стивен",
        MiddleName = "Эдвин",
        Id = 3
      }
    );
    authorDictionary.Add(
      4,
      new Author
      {
        FirstName = "Лермонтов",
        LastName = "Михаил",
        MiddleName = "Юрьевич",
        Id = 4
      }
    );

    bookEditions.Add(
      new BookEdition
      {
        Id = 1,
        ISBN = "978-5-87107-240-0",
        Title = "Пророк",
        Description = "Про пророка что-то",
        Authors = new List<Author>() { authorDictionary[1] },
        Genres = new List<Genre> { genreDictionary["драма"] }
      }
    );
    bookEditions.Add(
      new BookEdition
      {
        Id = 2,
        ISBN = "978-5-699-26845-0",
        Title = "Идиот",
        Description = "Про преступника что-то",
        Authors = new List<Author>() { authorDictionary[2] },
        Genres = new List<Genre> { genreDictionary["комедия"], genreDictionary["драма"] }
      }
    );
    bookEditions.Add(
      new BookEdition
      {
        Id = 3,
        ISBN = "978-5-17-123231-3",
        Title = "Оно",
        Description = "Про клоуна-педофила что-то",
        Authors = new List<Author>() { authorDictionary[3] },
        Genres = new List<Genre> { genreDictionary["ужасы"] }
      }
    );
    bookEditions.Add(
      new BookEdition
      {
        Id = 4,
        ISBN = "978-5-04-111308-7",
        Title = "Герой нашего времени",
        Description = "Про военного что-то",
        Authors = new List<Author>() { authorDictionary[4] },
        Genres = new List<Genre> { genreDictionary["трагедия"], genreDictionary["драма"] }
      }
    );
    bookEditions.Add(
      new BookEdition
      {
        Id = 5,
        ISBN = "978-5-17-080493-1",
        Title = "Сияние",
        Description = "Без понятия о чём оно",
        Authors = new List<Author>() { authorDictionary[3] },
        Genres = new List<Genre> { genreDictionary["ужасы"] }
      }
    );
  }

  public void Seed(ModelBuilder modelBuilder)
  {
    // modelBuilder.Entity<Genre>().HasData(genreDictionary.Values);
    // modelBuilder.Entity<Author>().HasData(authorDictionary.Values);
    // modelBuilder.Entity<BookEdition>().HasData(bookEditions);
  }

}
