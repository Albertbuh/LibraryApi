using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Library.API.Services;

public class JwtTokenService: ITokenService
{
  private const string ISSUER = "LibraryAPI";
  private const string AUDIENCE = "Users";
  const string KEY = "1234567890_authorize";

  public SymmetricSecurityKey GetSymmetricSecurityKey() =>
    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));

  public TokenValidationParameters GetTokenValidationParameters() =>
    new TokenValidationParameters
    {
      ValidateIssuer = true,
      ValidIssuer = ISSUER,
      ValidateAudience = true,
      ValidAudience = AUDIENCE,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = GetSymmetricSecurityKey()
    };

  public string GenerateTokenByClaims(List<Claim> claims, int time)
  {
    var token = new JwtSecurityToken(
          issuer: JwtAuthProvider.ISSUER,
          audience: JwtAuthProvider.AUDIENCE,
          claims: claims,
          expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(time)),
          signingCredentials: new SigningCredentials(JwtAuthProvider.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
          );
    return new JwtSecurityTokenHandler().WriteToken(token);
  }

  public string GetTokenByUsername(string? username)
  {
    if (username == null)
      username = "user";

    var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
    var jwt = JwtAuthProvider.GenerateJwt(claims, 180);
    return jwt;
  }
}
