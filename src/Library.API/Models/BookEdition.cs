namespace Library.API.Models;

public class BookEdition
{
  public int Id {get; set;}
  public string ISBN {get; set;} = null!;
  public string Title {get; set;} = null!;
  public string? Description {get; set;}
  public List<Author> Authors {get; set;} = new();
  public List<Genre> Genres {get; set;} = new();

  public BookEdition() { }

  public BookEdition(int id, string isbn, string title, string? description)
  {
    Id = id;
    ISBN = ISBN;
    Title = title;
    Description = description;
  }

}
