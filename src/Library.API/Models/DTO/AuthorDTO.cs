namespace Library.API.Models.DTO;

public class AuthorDTO
{
  public int Id { get; set; }
  public string FirstName { get; set; } = null!;
  public string? MiddleName { get; set; }
  public string? LastName { get; set; }
}
