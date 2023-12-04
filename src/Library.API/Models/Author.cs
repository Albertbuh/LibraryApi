namespace Library.API.Models;

public class Author
{
  public int Id { get; set; }
  public string FirstName { get; set; } = null!;
  public string? MiddleName { get; set; }
  public string? LastName { get; set; }
  public List<BookEdition> BookEditions { get; set; } = new();

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

  public override bool Equals(object? obj)
  {
    bool result = false;
    if (obj is Author a)
    {
      result = a.FirstName == this.FirstName
                && (a.MiddleName == this.MiddleName) || (a.MiddleName == null && this.MiddleName == null)
                && (a.LastName == this.LastName) || (a.LastName == null && this.LastName == null);
    }
    return result;
  }

  public override int GetHashCode()
  {
    unchecked
    {
      int hash = 1832;
      hash += 3 * this.FirstName.GetHashCode();
      hash += (this.MiddleName != null ? 7 * this.MiddleName.GetHashCode() : 0);
      hash += (this.LastName != null ? 12 * this.LastName.GetHashCode() : 0);
      return hash;
    }
  }

  public override string ToString()
  {
    return $"{FirstName} {LastName} {MiddleName}";
  }
}
