namespace Library.API.Models;

public class BookInstance
{
  public int Id {get; set;}
  public DateOnly? DateOfTaken {get; set;}
  public DateOnly? DateOfReturn {get; set;}

  public int BookEditionId {get; set;}
  public BookEdition Book {get; set;} = null!;
  public BookInstance() { }

  public BookInstance(BookEdition bookEdition)
  {
    Book = bookEdition;
  }
}
