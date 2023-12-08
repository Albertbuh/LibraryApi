using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public static class JwtAuthProvider
{
  public const string ISSUER = "LibraryAPI";
  public const string AUDIENCE = "Users";
  const string KEY = "1234567890_authorize";

  public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));

  public static TokenValidationParameters GetTokenValidationParameters() =>
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

  public static string GenerateJwt(List<Claim> claims, int time)
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

}
