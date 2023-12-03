namespace Library.API.Models;

public class Author
{
  public int Id { get; set; }
  public string FirstName { get; set; } = null!;
  public string? MiddleName { get; set; }
  public string? LastName { get; set; }
  public List<BookEdition> Books { get; set; } = new();

  public Author() { }

  public Author(string firstname)
  {
    FirstName = firstname;
  }

  public Author(string firstname, string lastname)
    : this(firstname)
  {
    LastName = lastname;
  }

  public Author(string firstname, string lastname, string middlename)
    : this(firstname, lastname)
  {
    MiddleName = middlename;
  }
}
