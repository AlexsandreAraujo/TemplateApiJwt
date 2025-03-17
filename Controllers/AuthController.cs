using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TemplateApiJwt.Application.Services;
using TemplateApiJwt.Domain.Model.EmployeeAggregate;

namespace TemplateApiJwt.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public ActionResult<dynamic> Auth(string username, string password)
        {
            // var user = _userRepository.Get(FromBodyAttribute.username, FromBodyAttribute.password);
            // if (user == null)
            //     return BadRequest("username or password is incorrect");

            if (username == "user" && password == "admin")
            {
                var token = TokenService.GenerateToken(_configuration, new Employee(username, 21, null));
                var refreshToken = TokenService.GenerateRefreshToken();
                TokenService.SaveRefreshToken(username, refreshToken);
                return new
                {
                    user = username,
                    token = token,
                    refreshToken = refreshToken
                };
            }

            return BadRequest("username or password is incorrect");
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh(string token, string refreshToken)
        {
            var principal = TokenService.GetPrincipalFromExpiredToken(_configuration, token);
            var username = principal.Identity.Name;
            var savedRefreshToken = TokenService.GetRefreshToken(username);
            if (savedRefreshToken != refreshToken)
                throw new SecurityTokenException("Invalid resfresh token");

            var newJwtToken = TokenService.GenerateToken(_configuration, principal.Claims);
            var newRefreshToken = TokenService.GenerateRefreshToken();
            TokenService.DeleteRefreshToken(username, refreshToken);
            TokenService.SaveRefreshToken(username, newRefreshToken);

            return new ObjectResult(new
            {
                user = username,
                token = newJwtToken,
                refreshToken = newRefreshToken
            });


        }
    }
}
