using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using TemplateApiJwt.Domain.Model.EmployeeAggregate;
using System.Security.Cryptography;

namespace TemplateApiJwt.Application.Services;

public class TokenService
{
  public static object GenerateToken(IConfiguration configuration, Employee employee)
  {
    var keyDecoded = configuration["Configurations:TokenConfigurations:Key"];
    var tokenExpiresInMinutes = int.Parse(configuration["Configurations:TokenConfigurations:TokenExpiresInMinutes"]);
    var key = Encoding.ASCII.GetBytes(keyDecoded);
    var tokenConfig = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(new Claim[]
      {
        new Claim("employeeId", employee.id.ToString()),
        new Claim(ClaimTypes.Name, employee.name)
      }),
      Expires = DateTime.UtcNow.AddMinutes(tokenExpiresInMinutes),
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenConfig);
    var tokenString = tokenHandler.WriteToken(token);

    return new
    {
      token = tokenString
    };
  }

  public static object GenerateToken(IConfiguration configuration, IEnumerable<Claim> claims)
  {
    var keyDecoded = configuration["Configurations:TokenConfigurations:Key"];
    var tokenExpiresInMinutes = int.Parse(configuration["Configurations:TokenConfigurations:TokenExpiresInMinutes"]);

    var key = Encoding.ASCII.GetBytes(keyDecoded);
    var tokenConfig = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.UtcNow.AddMinutes(tokenExpiresInMinutes),
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenConfig);
    var tokenString = tokenHandler.WriteToken(token);

    return new
    {
      token = tokenString
    };
  }

  public static string GenerateRefreshToken()
  {
    var randomNumber = new byte[32];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomNumber);
    return Convert.ToBase64String(randomNumber);
  }

  public static ClaimsPrincipal GetPrincipalFromExpiredToken(IConfiguration configuration, string token)
  {
    var tokenValidationParameters = new TokenValidationParameters
    {
      ValidateAudience = false,
      ValidateIssuer = false,
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Configurations:TokenConfigurations:Key"])),
      ValidateLifetime = false
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
    var jwtSecurityToken = securityToken as JwtSecurityToken;
    if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
      throw new SecurityTokenException("Invalid token");

    return principal;
  }

  private static List<(string, string)> _refreshTokens = new();

  public static void SaveRefreshToken(string username, string refreshToken)
  {
    _refreshTokens.Add(new(username, refreshToken));
  }

  public static string GetRefreshToken(string username)
  {
    return _refreshTokens.FirstOrDefault(x => x.Item1 == username).Item2;
  }

  public static void DeleteRefreshToken(string username, string refreshToken)
  {
    var item = _refreshTokens.FirstOrDefault(x => x.Item1 == username && x.Item2 == refreshToken);
    _refreshTokens.Remove(item);
  }
}
