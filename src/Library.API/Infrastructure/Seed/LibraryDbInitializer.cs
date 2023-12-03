using Library.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.API.Infrastructure.Seed;
public class LibraryDbInitializer : IDatabaseInitializer
{
  public LibraryDbInitializer()
  {}

  public void Seed(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Genre>().HasData(
        new Genre("комедия")  {Id = 1},
        new Genre("драма")    {Id = 2},
        new Genre("трагедия") {Id = 3},
        new Genre("ужасы")    {Id = 4}
        );
    
    
    modelBuilder.Entity<Author>().HasData(
        new Author("Пушкин", "Александр", "Сергеевич") {Id = 1},
        new Author("Достоевский", "Фёдор")             {Id = 2},
        new Author("Кинг", "Стивен")                   {Id = 3},
        new Author("Лермонтов", "Михаил")              {Id = 4}
        );
      
  }
}
