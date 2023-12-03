namespace Library.API.Models;

public class BookAuthor
{
  public int BookId {get; set;}
  public int AuthorId {get; set;}
  public BookEdition Book {get; set;} = null!;
  public Author Author {get; set;} = null!;
}
