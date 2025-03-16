using Microsoft.AspNetCore.Mvc;
using TemplateApiJwt.Services;

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
                var token = TokenService.GenerateToken(new Model.Employee(username, 23, null));
                return Ok(token);
            }

            return BadRequest("username or password is incorrect");
        }
    }
}
