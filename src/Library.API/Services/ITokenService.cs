using System.Security.Claims;

namespace Library.API.Services;

public interface ITokenService
{
  public string GetTokenByUsername(string? username);
  
  public string GenerateTokenByClaims(List<Claim> claims, int time);
 
}
