using BlogDataLibrary.Data;
using BlogDataLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ISqlData _db;

        public LoginController(IConfiguration config, ISqlData db)
        {
            _config = config;
            _db = db;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public ActionResult Login([FromBody] UserLogin login)
        {
            UserModel user = _db.Authenticate(login.UserName, login.Password);
            if (user != null)
            {
                var token = GenerateToken(user);
                // Return as JSON object instead of plain string
                return Ok(new { token = token });
            }
            return NotFound(new { message = "User not found" });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public ActionResult Register([FromBody] UserModel user)
        {
            if (user == null)
            {
                return BadRequest(new { message = "Invalid user data" });
            }
            try
            {
                _db.InsertUser(user);
                return Ok(new { message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                // Check if it's a duplicate username error
                if (ex.Message.Contains("Username already exists"))
                {
                    return BadRequest(new { message = "Username already exists" });
                }
                // Log other errors and return generic message
                return BadRequest(new { message = "Registration failed. Please try again." });
            }
        }

        private string GenerateToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            string userIdStr = user.Id.ToString();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userIdStr),
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}