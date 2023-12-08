namespace Library.API.Models;

public class Genre
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public List<BookEdition> BookEditions { get; set; } = new();

  public Genre() { }

  public Genre(string name)
  {
    Name = name;
  }


  public override bool Equals(object? obj)
  {
    if (obj is Genre g)
    {
      return g.Name.Equals(this.Name);
    }
    return false;
  }

  public override int GetHashCode()
  {
    return (23 * Name.GetHashCode()) % Int32.MaxValue; 
  }

  public override string ToString()
  {
    return $"{Name}";
  }
}
