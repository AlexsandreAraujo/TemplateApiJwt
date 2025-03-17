using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace TemplateApiJwt.Application.Extensions;

internal static class ServiceColletctionExtensions
{
  internal static IServiceCollection AddAuthenticationJwt(this IServiceCollection services, IConfiguration configuration)
  {
    var key = Encoding.ASCII.GetBytes(configuration["Configurations:TokenConfigurations:Key"]);
    services.AddAuthentication(x =>
    {
      x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
      x.RequireHttpsMetadata = false;
      x.SaveToken = true;
      x.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true, // Adiciona a validação do tempo de vida
        ClockSkew = TimeSpan.Zero // Define a tolerância de tempo para zero
      };
    });

    return services;
  }
}
