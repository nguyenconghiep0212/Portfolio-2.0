using Azure.Core;
using BackEnd.Contexts;
using BackEnd.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserContext _context; 
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration, UserContext userLoginContext)
        {
            _context = userLoginContext;
            _configuration = configuration; 
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            // Check if username already exists
            if (await _context.User.AnyAsync(u => u.Username == request.Username))
                return BadRequest("Username already exists.");

            // Hash the password
            PasswordHelper.CreatePasswordHash(request.Password, out byte[] hash, out byte[] salt);

            // Create new user
            var user = new User
            {
                Username = request.Username,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.User.SingleOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
                return Unauthorized("Invalid credentials.");

            bool isValid = PasswordHelper.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt);
            if (!isValid)
                return Unauthorized("Invalid credentials.");

            var token = GenerateJwtToken(user.Username);

            // Generate new refresh token and expiration
            var newRefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var newRefreshTokenExpiration = DateTime.UtcNow.AddDays(7);

            // Update user with new refresh token and expiration
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiration = newRefreshTokenExpiration;

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Token = token,
                RefreshToken = newRefreshToken
            });
        }

        [HttpPost("refresh_token")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refreshToken)
        {
            // Find user by refresh token
            var user = await _context.User.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken.RefreshToken);

            if (user == null)
                return Unauthorized();

            // Check if refresh token is expired
            if (user.RefreshTokenExpiration < DateTime.UtcNow)
                return Unauthorized();

            // Generate new JWT
            var newJwtToken = GenerateJwtToken(user.Username);

            // Generate new refresh token and expiration
            var newRefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var newRefreshTokenExpiration = DateTime.UtcNow.AddDays(7);

            // Update user with new refresh token and expiration
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiration = newRefreshTokenExpiration;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Token = newJwtToken,
                RefreshToken = newRefreshToken
            });
        }

        private string GenerateJwtToken(string username)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            string JwtSecretKey = _configuration["JWTKey:JwtSecretKey"];
            string ValidIssuer = _configuration["JWTKey:ValidIssuer"];
            string ValidAudience = _configuration["JWTKey:ValidAudience"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: ValidIssuer,
                audience: ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
