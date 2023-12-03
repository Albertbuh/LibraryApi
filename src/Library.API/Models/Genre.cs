namespace Library.API.Models;

public class Genre
{
  public int Id {get; set;}
  public string Name {get; set;} = null!;
  public List<BookEdition> Books {get; set;} = new();

  public Genre()
  {}

  public Genre(string name)
  {
    Name = name;
  }
}
