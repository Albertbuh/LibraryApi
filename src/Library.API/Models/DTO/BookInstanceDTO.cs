namespace Library.API.Models.DTO;

public class BookInstanceDTO
{
  public int Id {get; set;}
  public string ISBN {get; set;} = null!;
  public string Title {get; set;} = null!;
  public string? Description {get; set;}
  public List<AuthorDTO> Authors {get; set;} = new();
  public List<GenreDTO> Genres {get; set;} = new();
 
  public DateOnly? DateOfTaken {get; set;}
  public DateOnly? DateOfReturn {get; set;}

}
