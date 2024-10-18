using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using Lab_1.Models;

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
                return Unauthorized(new { errorText = "Invalid username or password." });
            }

            return AuthOptions.GenerateToken(identity);
        }
    }
}
