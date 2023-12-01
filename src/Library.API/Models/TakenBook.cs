namespace Library.API.Models;

public class TakenBook
{
  public int Id {get; set;}
  public Book Book {get; set;} = null!;
  public DateOnly DateOfTaken {get; set;}
  public DateOnly DateOfReturn {get; set;}
}
