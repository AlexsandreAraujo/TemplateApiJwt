using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Auth(string username, string password)
        {
            if (username == "admin" && password == "admin")
            {
                var token = TokenService.GenerateToken(_configuration, new Employee());
                return Ok(token);
            }

            return BadRequest("username or password is incorrect");
        }
    }
}
