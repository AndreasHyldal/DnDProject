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
    }
}
