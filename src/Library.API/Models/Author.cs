namespace Library.API.Models;

public class Author
{
  public int Id {get; set;}
  public string FirstName {get; set;} = null!;
  public string? MiddleName {get; set;}
  public string? LastName {get; set;}
  public List<Book> Books {get; set;} = new();
}
