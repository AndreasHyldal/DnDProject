using System;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly EmployeeService _employeeService;
        private readonly TokenService _tokenService;
        
        public AuthController(EmployeeService employeeService, TokenService tokenService)
        {
            _employeeService = employeeService;
            _tokenService = tokenService;
        }
        
        // DTO for login requests
        public class LoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid login request.");
            }
            
            // Use EmployeeService to authenticate the user.
            var employee = await _employeeService.AuthenticateEmployeeAsync(loginRequest.Email, loginRequest.Password);
            if (employee == null)
            {
                return Unauthorized("Invalid credentials.");
            }
            
            // Generate JWT token (this token will include the employee's role as a claim)
            var token = _tokenService.GenerateToken(employee);
            return Ok(new { token });
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
