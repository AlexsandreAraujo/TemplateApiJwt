using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using TemplateApiJwt.Domain.Model.EmployeeAggregate;

namespace TemplateApiJwt.Application.Services;

public class TokenService
{
  public static object GenerateToken(IConfiguration configuration, Employee employee)
  {
    var key = Encoding.ASCII.GetBytes(configuration["Configurations:TokenConfigurations:Key"]);
    var tokenConfig = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(new Claim[]
      {
        new Claim("employeeId", employee.id.ToString()),
      }),
      Expires = DateTime.UtcNow.AddHours(1),
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
}
