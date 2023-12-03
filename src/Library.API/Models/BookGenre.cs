namespace Library.API.Models;

public class BookGenre
{
  public int BookId {get; set;}
  public int GenreId {get; set;}
  public BookEdition Book {get; set;} = null!;
  public Genre Genre {get; set;} = null!;
}
