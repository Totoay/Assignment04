using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AspNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private const string TokenSecret = "ABDBB766778D36F4EE5AC70253528EA241B7FC18789CC6F07C5D71AF0C99CDA9";

        [HttpPost("loginUser")]
        public IActionResult LoginUser()
        {
            var token = GenerateToken("user");
            return Ok(new { token });
        }

        [HttpPost("loginAdmin")]
        public IActionResult LoginAdmin()
        {
            var token = GenerateToken("admin");
            return Ok(new { token });
        }

        private string GenerateToken(string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Role, role)
        };

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
