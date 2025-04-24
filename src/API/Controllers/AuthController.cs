using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IConfiguration _configuration;

		public AuthController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[HttpPost("login")]
		public IActionResult Login([FromBody] LoginModel loginModel)
		{
			// Validate user credentials here (e.g., check username and password)
			if (loginModel.Username != "admin" || loginModel.Password != "password") // Basic validation
			{
				return Unauthorized();
			}

			// Create JWT token
			var claims = new[]
			{
			new Claim(ClaimTypes.Name, loginModel.Username),
			new Claim(ClaimTypes.Role, "Admin") // Adding roles if needed
        };

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _configuration["JwtSettings:Issuer"],
				audience: _configuration["JwtSettings:Audience"],
				claims: claims,
				expires: DateTime.Now.AddMinutes(30), // Token expiration time
				signingCredentials: creds
			);

			var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

			return Ok(new { Token = tokenString });
		}
	}

	public class LoginModel
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}
}
