using Microsoft.AspNetCore.Mvc;

namespace Lab_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        public struct LoginData
        {
            public string login { get; set; }
            public string password { get; set; }
        }

        [HttpPost] // api/Account/token
        public async Task<ActionResult<object>> GetToken([FromBody] LoginData ld)
        {
            var identity = AuthOptions.GetIdentity(ld.login, ld.password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            return AuthOptions.GenerateToken(identity);
        }
    }
}
