using Backend.Models; 
using Backend.Services; 
using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly EmployeeService _employeeService;
        private readonly IConfiguration _configuration;

        public AuthController(EmployeeService employeeService, IConfiguration configuration)
        {
            _employeeService = employeeService;
            _configuration = configuration;
        }

        /// <summary>
        /// Endpoint to authenticate a user and retrieve a JWT token.
        /// </summary>
        /// <param name="loginRequest">Object containing Email and Password</param>
        /// <returns>JWT Token if successful</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid login request.");
            }

            // Authenticate using the EmployeeService
            var employee = await _employeeService.AuthenticateEmployeeAsync(
                loginRequest.Email,
                loginRequest.Password
            );

            if (employee == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            // Generate JWT token
            var token = GenerateJwtToken(employee);

            return Ok(new { token });
        }

        [HttpGet("user-id")]
        public async Task<IActionResult> GetUserId(string email)
        {

            var userId = await _employeeService.GetUserIdFromEmail(email);


            if (userId == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            return Ok(userId);
        }

        /// <summary>
        /// Generates the JWT token containing the employee's Email and Role
        /// </summary>
        /// <param name="employee">The authenticated employee</param>
        /// <returns>Signed JWT token as a string</returns>
        private string GenerateJwtToken(Employee employee)
        {
            // Retrieve the signing key from configuration
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT Key is missing in configuration.");

            // Create security key and signing credentials
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Add standard claims plus the role and ID claims.
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, employee.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, employee.Role ?? "User"),
                new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString())
            };

            // Create the token
            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddHours(1), // token validity (example: 1 hour)
                signingCredentials: credentials
            );

            // Return the serialized token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }    
    }

    /// <summary>
    /// Simple DTO to receive login credentials
    /// </summary>
    public class LoginRequest
    {
        public string Email { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
    }
}
