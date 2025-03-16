using Microsoft.AspNetCore.Mvc;
using TemplateApiJwt.Application.Services;
using TemplateApiJwt.Domain.Model.EmployeeAggregate;

namespace TemplateApiJwt.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : Controller
    {
        [HttpPost]
        public IActionResult Auth(string username, string password)
        {
            if (username == "admin" && password == "admin")
            {
                var token = TokenService.GenerateToken(new Employee());
                return Ok(token);
            }

            return BadRequest("username or password is incorrect");
        }
    }
}
